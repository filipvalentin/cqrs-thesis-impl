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
		IProjectReadSideRepository projectReadRepository,
		ICommentRepository commentRepository,
		IEventQueueService queueService,
		IPublisher publisher,
		ILogger<TaskDeletedDomainEventHandler> logger) : INotificationHandler<TaskDeletedDomainEvent> {

		private readonly ITaskReadSideRepository taskReadRepository = taskReadRepository;
		private readonly IProjectReadSideRepository projectReadRepository = projectReadRepository;
		private readonly ICommentRepository commentRepository = commentRepository;
		private readonly IEventQueueService queueService = queueService;
		private readonly IPublisher publisher = publisher;
		private readonly ILogger<TaskDeletedDomainEventHandler> logger = logger;


		public async Task Handle(TaskDeletedDomainEvent domainEvent, CancellationToken cancellationToken) {
			if (!domainEvent.Cascaded) {
				var projectResult = await projectReadRepository.FindByIdAsync(domainEvent.ProjectId);
				if (!projectResult.IsSuccess) {
					logger.LogError("Failed to find project with id {projectId}", domainEvent.ProjectId);
					queueService.Enqueue(domainEvent);
					return;
				}
				projectResult.Value.TaskIds.Remove(domainEvent.Id);
				var projectUpdateResult = await projectReadRepository.UpdateAsync(domainEvent.ProjectId, projectResult.Value);
				if (!projectUpdateResult.IsSuccess) {
					logger.LogError("Failed to update project with id {projectId}", domainEvent.ProjectId);
					queueService.Enqueue(domainEvent);
					return;
				}
			}

			/*Idempotent since a deleted comment will not be deleted again if the database drops for example*/
			var commentIds = new List<Guid>(domainEvent.CommentIds);
			while (commentIds.Count > 0) {
				var commentId = commentIds.First();
				var commentResult = await commentRepository.DeleteAsync(commentId);
				if (!commentResult.IsSuccess) {
					logger.LogError("Failed to remove comment with id {commentId}. Error: {Error}", commentId, commentResult.Error);
					queueService.Enqueue(domainEvent with { CommentIds = commentIds });
					return;
				}
				await publisher.Publish(
					new CommentDeletedDomainEvent(Id: commentId, Cascaded: true, TaskId: domainEvent.Id),
					cancellationToken);
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
