using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Comments.Events {
	public class CommentEditedDomainEventHandler(
		ICommentReadSideRepository commentReadRepository,
		ILogger<CommentEditedDomainEventHandler> logger,
		IMapper mapper,
		IEventQueueService queueService) : INotificationHandler<CommentEditedDomainEvent> {

		private readonly ICommentReadSideRepository commentReadRepository = commentReadRepository;
		private readonly ILogger<CommentEditedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(CommentEditedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var commentReadResult = await commentReadRepository.UpdateAsync(domainEvent.Id, mapper.Map<CommentReadModel>(domainEvent));
			if (!commentReadResult.IsSuccess) {
				logger.LogError("Error from CommentReadSideRepository: {Error} when updating entity with {Id}", commentReadResult.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
