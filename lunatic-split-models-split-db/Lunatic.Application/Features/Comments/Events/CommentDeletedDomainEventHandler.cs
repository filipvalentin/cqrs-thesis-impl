using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Comments.Events {
	public class CommentDeletedDomainEventHandler :INotificationHandler<CommentDeletedDomainEvent>{
		private readonly ICommentReadSideRepository commentReadRepository;
		private readonly ILogger<CommentDeletedDomainEventHandler> logger;

		public CommentDeletedDomainEventHandler(ICommentReadSideRepository commentReadRepository, ILogger<CommentDeletedDomainEventHandler> logger) {
			this.commentReadRepository = commentReadRepository;
			this.logger = logger;
		}

		public async Task Handle(CommentDeletedDomainEvent notification, CancellationToken cancellationToken) {
			var commentReadRemovedResult = await commentReadRepository.DeleteAsync(notification.Id);

			if (!commentReadRemovedResult.IsSuccess) {
				logger.LogError("Failed to remove comment read model with id {Id}", notification.Id);
			}
		}

	}
}
