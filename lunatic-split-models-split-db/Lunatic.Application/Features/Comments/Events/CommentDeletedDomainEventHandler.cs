using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Comments.Events {
	public class CommentDeletedDomainEventHandler(
		ICommentReadSideRepository commentReadRepository,
		ILogger<CommentDeletedDomainEventHandler> logger,
		IEventQueueService queueService,
		ITaskReadSideRepository taskReadRepository) : INotificationHandler<CommentDeletedDomainEvent> {

		private readonly ICommentReadSideRepository commentReadRepository = commentReadRepository;
		private readonly ITaskReadSideRepository taskReadRepository = taskReadRepository;
		private readonly ILogger<CommentDeletedDomainEventHandler> logger = logger;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(CommentDeletedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var commentReadRemovedResult = await commentReadRepository.DeleteAsync(domainEvent.Id);
			if (!commentReadRemovedResult.IsSuccess) {
				logger.LogError("Error from CommentReadSideRepository: {Error} when removing entity with {Id}", commentReadRemovedResult.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}

	}
}
