using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Teams.Events {
	internal class TeamDisbandedDomainEventHandler(
		ITeamReadSideRepository teamRepository,
		IProjectRepository projectWriteRepository,
		ILogger<TeamDisbandedDomainEventHandler> logger,
		IPublisher publisher,
		IEventQueueService eventQueueService) : INotificationHandler<TeamDisbandedDomainEvent> {

		private readonly ITeamReadSideRepository teamReadRepository = teamRepository;
		private readonly IProjectRepository projectWriteRepository = projectWriteRepository;
		private readonly ILogger<TeamDisbandedDomainEventHandler> logger = logger;
		private readonly IPublisher publisher = publisher;
		private readonly IEventQueueService eventQueueService = eventQueueService;

		public async Task Handle(TeamDisbandedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var projectIds = domainEvent.ProjectIds;
			while (projectIds.Count > 0) {
				var projectId = projectIds.First();
				var projectReadResult = await projectWriteRepository.FindByIdAsync(projectId);
				if (!projectReadResult.IsSuccess) {
					logger.LogError("Failed to find project with id {projectId}", projectId);
					eventQueueService.Enqueue(domainEvent with { ProjectIds = projectIds });
					return;
				}
				var projectRemovedResult = await projectWriteRepository.DeleteAsync(projectId);
				if (!projectRemovedResult.IsSuccess) {
					logger.LogError("Failed to remove project with id {projectId}", projectId);
					eventQueueService.Enqueue(domainEvent with { ProjectIds = projectIds });
					return;
				}
				await publisher.Publish(
					new ProjectDeletedDomainEvent(Id: projectId, TaskIds: projectReadResult.Value.TaskIds),
					cancellationToken);
				projectIds.RemoveAt(0);
			}

			var teamReadRemovedResult = await teamReadRepository.DeleteAsync(domainEvent.Id);
			if (!teamReadRemovedResult.IsSuccess) {
				logger.LogError("Failed to remove team read model with id {Id}", domainEvent.Id);
				eventQueueService.Enqueue(domainEvent);
				return;
			}
		}
	}
}
