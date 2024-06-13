using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Comment;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Tasks.Events {
	public class TaskDeletedDomainEventHandler(
		ITaskReadSideRepository taskReadRepository,
		ICommentRepository commentWriteRepository,
		IEventQueueService queueService,
		IPublisher publisher,
		ILogger<TaskDeletedDomainEventHandler> logger,
		IUnitOfWork unitOfWork) : INotificationHandler<TaskDeletedDomainEvent> {

		private readonly ITaskReadSideRepository taskReadRepository = taskReadRepository;
		private readonly ICommentRepository commentWriteRepository = commentWriteRepository;
		private readonly IUnitOfWork unitOfWork = unitOfWork;
		private readonly IEventQueueService queueService = queueService;
		private readonly IPublisher publisher = publisher;
		private readonly ILogger<TaskDeletedDomainEventHandler> logger = logger;


		public async Task Handle(TaskDeletedDomainEvent domainEvent, CancellationToken cancellationToken) {

			/*Idempotent since a deleted comment will not be deleted again if the database drops for example*/
			var commentIds = new List<Guid>(domainEvent.CommentIds);
			while (commentIds.Count > 0) {
				var commentId = commentIds.First();
				var commentResult = await commentWriteRepository.DeleteAsync(commentId);
				if (!commentResult.IsSuccess) {
					logger.LogError("Failed to remove comment with id {commentId}. Error: {Error}", commentId, commentResult.Error);
					queueService.Enqueue(domainEvent with { CommentIds = commentIds });
					return;
				}
				await unitOfWork.SaveChangesAsync(cancellationToken);
				await publisher.Publish(new CommentDeletedDomainEvent(Id: commentId), cancellationToken);
				commentIds.RemoveAt(0);
			}


			var taskDeletedResult = await taskReadRepository.DeleteAsync(domainEvent.Id);
			if (!taskDeletedResult.IsSuccess) {
				logger.LogError("Failed to remove task with id {taskId}", domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
