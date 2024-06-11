using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Comments.Events {
	public class CommentAddedDomainEventHandler(
		ICommentReadSideRepository commentReadRepository,
		ICommentRepository commentWriteRepository, 
		ILogger<CommentAddedDomainEventHandler> logger, 
		IMapper mapper, 
		ITaskReadSideRepository taskReadRepository, 
		IEventQueueService queueService) : INotificationHandler<CommentAddedDomainEvent> {
		private readonly ICommentReadSideRepository commentReadRepository = commentReadRepository;
		private readonly ITaskReadSideRepository taskReadRepository = taskReadRepository;
		private readonly ICommentRepository commentWriteRepository = commentWriteRepository;
		private readonly ILogger<CommentAddedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(CommentAddedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var taskResult = await taskReadRepository.FindByIdAsync(domainEvent.TaskId);
			if (!taskResult.IsSuccess) {
				logger.LogError("Error from TaskReadSideRepository: {Error} when searching for {Id}", taskResult.Error, domainEvent.TaskId);
				queueService.Enqueue(domainEvent);
				return;
			}

			taskResult.Value.CommentIds.Add(domainEvent.Id);

			var taskUpdateResult = await taskReadRepository.UpdateAsync(taskResult.Value.Id, taskResult.Value);
			if (!taskUpdateResult.IsSuccess) {
				logger.LogError("Error from TaskReadSideRepository: {Error} when updating entity with {Id}", taskUpdateResult.Error, taskResult.Value.Id);
				queueService.Enqueue(domainEvent);
				return;
			}

			var status = await commentReadRepository.AddAsync(mapper.Map<CommentReadModel>(domainEvent));
			if (!status.IsSuccess) {
				logger.LogError("Error from CommentReadSideRepository: {Error} when adding entity with {Id}", status.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
