using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Comments.Events {
	public class CommentDeletedDomainEventHandler(ICommentReadSideRepository commentReadRepository,
		ILogger<CommentDeletedDomainEventHandler> logger,
		IEventQueueService queueService,
		ITaskReadSideRepository taskReadRepository) : INotificationHandler<CommentDeletedDomainEvent> {

		private readonly ICommentReadSideRepository commentReadRepository = commentReadRepository;
		private readonly ITaskReadSideRepository taskReadRepository = taskReadRepository;
		private readonly ILogger<CommentDeletedDomainEventHandler> logger = logger;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(CommentDeletedDomainEvent notification, CancellationToken cancellationToken) {
			var taskResult = await taskReadRepository.FindByIdAsync(notification.TaskId);
			if (!taskResult.IsSuccess) {
				logger.LogError("Error from TaskReadSideRepository: {Error} when searching for {Id}", taskResult.Error, notification.TaskId);
				queueService.Enqueue(notification);
				return;
			}

			taskResult.Value.CommentIds.Remove(notification.Id);
			var taskUpdateResult = await taskReadRepository.UpdateAsync(taskResult.Value.Id, taskResult.Value);
			if (!taskUpdateResult.IsSuccess) {
				logger.LogError("Error from TaskReadSideRepository: {Error} when updating entity with {Id}", taskUpdateResult.Error, taskResult.Value.Id);
				queueService.Enqueue(notification);
				return;
			}

			var commentReadRemovedResult = await commentReadRepository.DeleteAsync(notification.Id);
			if (!commentReadRemovedResult.IsSuccess) {
				logger.LogError("Error from CommentReadSideRepository: {Error} when removing entity with {Id}", commentReadRemovedResult.Error, notification.Id);
				queueService.Enqueue(notification);
			}
		}

	}
}
