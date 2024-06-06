using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Teams.Events {
	internal class TeamDisbandedDomainEventHandler : INotificationHandler<TeamDisbandedDomainEvent> {
		private readonly ITeamReadSideRepository teamReadRepository;
		private readonly IProjectRepository projectWriteRepository;
		private readonly ILogger<TeamDisbandedDomainEventHandler> logger;
		private readonly IPublisher publisher;

		public TeamDisbandedDomainEventHandler(ITeamReadSideRepository teamRepository,
			IProjectRepository projectWriteRepository, ILogger<TeamDisbandedDomainEventHandler> logger, IPublisher publisher) {
			this.teamReadRepository = teamRepository;
			this.projectWriteRepository = projectWriteRepository;
			this.logger = logger;
			this.publisher = publisher;
		}

		public async Task Handle(TeamDisbandedDomainEvent notification, CancellationToken cancellationToken) {
			var teamReadRemovedResult = await teamReadRepository.DeleteAsync(notification.Id);

			if (!teamReadRemovedResult.IsSuccess) {
				logger.LogError("Failed to remove team read model with id {Id}", notification.Id);
			}

			var team = notification.Team;
			foreach (var projectId in team.ProjectIds) {
				var projectReadResult = await projectWriteRepository.FindByIdAsync(projectId);
				if (!projectReadResult.IsSuccess) {
					logger.LogError("Failed to find project with id {projectId}", projectId);
				}
				var projectRemovedResult = await projectWriteRepository.DeleteAsync(projectId);
				if (!projectRemovedResult.IsSuccess) {
					logger.LogError("Failed to remove project with id {projectId}", projectId);
				}
				await publisher.Publish(new ProjectDeletedDomainEvent(projectId, projectReadResult.Value), cancellationToken);
			}
		}
	}
}
