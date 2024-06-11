using AutoMapper;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Application.Persistence.ReadSide;
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
		private readonly IProjectReadSideRepository	projectReadRepository;
		private readonly ILogger<TaskCreatedDomainEventHandler> logger;
		private readonly IMapper mapper;

		public TaskCreatedDomainEventHandler(ITaskRepository taskWriteRepository,
			ITaskReadSideRepository taskReadRepository, ILogger<TaskCreatedDomainEventHandler> logger, IMapper mapper, IProjectReadSideRepository projectReadRepository) {
			this.taskWriteRepository = taskWriteRepository;
			this.taskReadRepository = taskReadRepository;
			this.logger = logger;
			this.mapper = mapper;
			this.projectReadRepository = projectReadRepository;
		}

		public async Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken) {
			var taskResult = await taskWriteRepository.FindByIdAsync(notification.Id);
			if (!taskResult.IsSuccess) {
				logger.LogError("Task with id {Id} not found", notification.Id);
				return;
			}

			var projectResult = await projectReadRepository.FindByIdAsync(taskResult.Value.ProjectId);
			if (!projectResult.IsSuccess) {
				logger.LogError("Project with id {Id} not found", taskResult.Value.ProjectId);
				return;
			}

			var project = projectResult.Value;
			project.TaskIds.Add(taskResult.Value.Id);
			var projectUpdateResult = await projectReadRepository.UpdateAsync(project.Id, project);
			if (!projectUpdateResult.IsSuccess) {
				logger.LogError("Failed to update project with id {Id} in read side", project.Id);
				return;
			}

			var status = await taskReadRepository.AddAsync( mapper.Map<TaskReadModel>(taskResult.Value));

			if (!status.IsSuccess) {
				logger.LogError("Failed to add task with id {Id} to read side", notification.Id);
			}
		}
	}
}
