using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Tasks.Events
{
    internal class TaskCreatedDomainEventHandler : INotificationHandler<TaskCreatedDomainEvent> {
		private readonly ITaskRepository taskWriteRepository;
		private readonly ITaskReadSideRepository taskReadRepository;
		private readonly ILogger<TaskCreatedDomainEventHandler> logger;

		public TaskCreatedDomainEventHandler(ITaskRepository taskWriteRepository,
			ITaskReadSideRepository taskReadRepository, ILogger<TaskCreatedDomainEventHandler> logger) {
			this.taskWriteRepository = taskWriteRepository;
			this.taskReadRepository = taskReadRepository;
			this.logger = logger;
		}

		public async Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken) {
			var taskResult = await taskWriteRepository.FindByIdAsync(notification.TaskId);

			if (!taskResult.IsSuccess) {
				logger.LogError($"Task with id {notification.TaskId} not found");
				return;
			}

			var task = taskResult.Value;

			var status = await taskReadRepository.AddAsync(new TaskReadModel {
				Id = task.Id,
				ProjectId = task.ProjectId,
				CreatedByUserId = task.CreatedByUserId,
				TaskSectionCard = task.TaskSectionCard,
				Title = task.Title,
				Description = task.Description,
				Priority = task.Priority,
				Status = task.Status,
				Tags = task.Tags,
				CommentIds = task.CommentIds,
				AssigneeIds = task.AssigneeIds,
				PlannedStartDate = task.PlannedStartDate,
				PlannedEndDate = task.PlannedEndDate,
				StartedDate = task.StartedDate,
				EndedDate = task.EndedDate
			});

			if (!taskResult.IsSuccess) {
				logger.LogError($"Failed to add task with id {notification.TaskId} to read side");
			}
		}
	}
}
