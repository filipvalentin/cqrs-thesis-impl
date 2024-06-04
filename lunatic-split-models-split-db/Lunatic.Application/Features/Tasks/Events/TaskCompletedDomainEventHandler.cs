using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Task;
using Lunatic.Domain.Entities;
using Lunatic.Domain.MLModel;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace Lunatic.Application.Features.Tasks.Events {
	internal class TaskCompletedDomainEventHandler : INotificationHandler<TaskCompletedDomainEvent> {
		private readonly IMLDataStorageService mlDataStorageService;

		private readonly ITaskRepository taskRepository;

		private readonly ICommentRepository commentRepository;

		public TaskCompletedDomainEventHandler(
			IMLDataStorageService mlDataStorageService,
			ITaskRepository taskRepository,
			ICommentRepository commentRepository
		) {
			this.mlDataStorageService = mlDataStorageService;
			this.taskRepository = taskRepository;
			this.commentRepository = commentRepository;
		}

		public async Task Handle(TaskCompletedDomainEvent notification, CancellationToken cancellationToken) {
			var taskResult = await taskRepository.FindByIdAsync(notification.TaskId);

			if (!taskResult.IsSuccess) {
				return;
			}

			var task = taskResult.Value;

			List<Comment> comments = (
				await Task.WhenAll(
					task.CommentIds.Select(
						async (comment) => {
							var commentResult = await commentRepository.FindByIdAsync(comment);
							return commentResult.IsSuccess ? commentResult.Value : null;
						}
					)
				)
			).Where(comment => comment != null).Select(comment => comment!).ToList();

			await mlDataStorageService.AddEntryAsync(
				new DaysToCompleteTaskEntry {
					TaskId = task.Id.ToString(),
					AssigneesCount = task.AssigneeIds.Count,
					DescriptionLength = task.Description.Length,
					CommentsCount = comments.Count,
					AverageCommentLength = comments.Count > 0 ? comments.Average(comment => comment.Content.Length) : 0,
					Priority = task.Priority,
					ExpectedDaysToComplete = (task.PlannedEndDate - task.PlannedStartDate).Days,
					DaysTookToComplete = (task.EndedDate!.Value - task.StartedDate!.Value).Days
				}
			);
		}
	}
}
