using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Comments.Events {
	public class CommentEditedDomainEventHandler(
		ICommentReadSideRepository commentReadRepository,
		ILogger<CommentEditedDomainEventHandler> logger,
		IMapper mapper,
		IEventQueueService queueService,
		IFlatTaskReadSideRepository flatTaskReadRepository) : INotificationHandler<CommentEditedDomainEvent> {

		private readonly ICommentReadSideRepository commentReadRepository = commentReadRepository;
		private readonly IFlatTaskReadSideRepository flatTaskReadRepository = flatTaskReadRepository;
		private readonly ILogger<CommentEditedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(CommentEditedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var commentReadModel = new CommentReadModel {
				Id = domainEvent.Id,
				TaskId = domainEvent.TaskId,
				CreatedByUserId = domainEvent.CreatedByUserId,
				Content = domainEvent.Content,
				CreatedDate = domainEvent.CreatedDate,
				LastModifiedDate = domainEvent.LastModifiedDate
			};
			var commentReadResult = await commentReadRepository.UpdateAsync(domainEvent.Id, commentReadModel);
			if (!commentReadResult.IsSuccess) {
				logger.LogError("Error from CommentReadSideRepository: {Error} when updating entity with {Id}", commentReadResult.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
				return;
			}

			var flatTaskReadResult = await flatTaskReadRepository.FindByIdAsync(domainEvent.TaskId);
			if (!flatTaskReadResult.IsSuccess) {
				logger.LogError("Error from FlatTaskReadSideRepository: {Error} when finding entity with {Id}", flatTaskReadResult.Error, domainEvent.TaskId);
				queueService.Enqueue(domainEvent);
				return;
			}
			var flatTask = flatTaskReadResult.Value;
			var commentIndex = flatTask.Comments.FindIndex(c => c.Id == domainEvent.Id);
			flatTask.Comments[commentIndex] = commentReadModel;
			var flatUpdateResult = await flatTaskReadRepository.UpdateAsync(domainEvent.TaskId, flatTask);
			if (!flatUpdateResult.IsSuccess) {
				logger.LogError("Error from FlatTaskReadSideRepository: {Error} when updating entity with {Id}", flatUpdateResult.Error, domainEvent.TaskId);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
