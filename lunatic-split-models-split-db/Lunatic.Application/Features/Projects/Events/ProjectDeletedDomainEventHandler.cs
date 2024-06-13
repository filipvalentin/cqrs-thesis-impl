using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Projects.Events {
	public class ProjectDeletedDomainEventHandler(
		IProjectReadSideRepository projectReadRepository,
		ITaskRepository taskWriteRepository,
		IUnitOfWork unitOfWork,
		ILogger<ProjectDeletedDomainEventHandler> logger,
		IPublisher publisher,
		IEventQueueService queueService,
		ITeamReadSideRepository teamReadRepository) : INotificationHandler<ProjectDeletedDomainEvent> {

		private readonly IProjectReadSideRepository projectReadRepository = projectReadRepository;
		private readonly ITaskRepository taskWriteRepository = taskWriteRepository;
		private readonly IUnitOfWork unitOfWork = unitOfWork;
		private readonly ITeamReadSideRepository teamReadRepository = teamReadRepository;
		private readonly ILogger<ProjectDeletedDomainEventHandler> logger = logger;
		private readonly IPublisher publisher = publisher;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(ProjectDeletedDomainEvent domainEvent, CancellationToken cancellationToken) {

			var taskIds = new List<Guid>(domainEvent.TaskIds);
			while (taskIds.Count > 0) {
				var taskId = taskIds[0];
				var taskResult = await taskWriteRepository.FindByIdAsync(taskId);
				if (!taskResult.IsSuccess) {
					logger.LogError("Failed to find task with id {taskId}", taskId);
					queueService.Enqueue(domainEvent with { TaskIds = taskIds });
					return;
				}
				var taskRemovedResult = await taskWriteRepository.DeleteAsync(taskId);
				if (!taskRemovedResult.IsSuccess) {
					logger.LogError("Failed to remove task with id {taskId}", taskId);
					queueService.Enqueue(domainEvent with { TaskIds = taskIds });
					return;
				}
				await unitOfWork.SaveChangesAsync(cancellationToken);
				await publisher.Publish(
					new TaskDeletedDomainEvent(Id: taskId, CommentIds: taskResult.Value.CommentIds),
					cancellationToken);
				taskIds.RemoveAt(0);
			}

			var projectReadRemovedResult = await projectReadRepository.DeleteAsync(domainEvent.Id);
			if (!projectReadRemovedResult.IsSuccess) {
				logger.LogError("Failed to remove project read model with id {Id}", domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}

		}
	}
}
