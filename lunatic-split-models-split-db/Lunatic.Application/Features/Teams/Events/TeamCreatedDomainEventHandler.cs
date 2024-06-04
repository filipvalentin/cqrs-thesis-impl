using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Teams.Events {
	public class TeamCreatedDomainEventHandler : INotificationHandler<TeamCreatedDomainEvent> {
		ITeamReadSideRepository teamReadRepository;
		ITeamRepository teamWriteRepository;
		ILogger<TeamCreatedDomainEventHandler> logger;

		public TeamCreatedDomainEventHandler(ITeamReadSideRepository teamRepository,
			ITeamRepository teamWriteRepository, ILogger<TeamCreatedDomainEventHandler> logger) {
			this.teamReadRepository = teamRepository;
			this.teamWriteRepository = teamWriteRepository;
			this.logger = logger;
		}

		public async Task Handle(TeamCreatedDomainEvent notification, CancellationToken cancellationToken) {
			var teamResult = await teamWriteRepository.FindByIdAsync(notification.TeamId);

			if (!teamResult.IsSuccess) {
				logger.LogError("Team not found in write side repository. TeamId: {TeamId}", notification.TeamId);
				return;
			}

			var team = teamResult.Value;

			var addResult = await teamReadRepository.AddAsync(new TeamReadModel {
				Id = team.Id,
				CreatedByUserId = team.CreatedByUserId,
				Name = team.Name,
				Description = team.Description,
				MemberIds = team.MemberIds,
				ProjectIds = team.ProjectIds
			});

			if (!addResult.IsSuccess) {
				logger.LogError("Failed to add team to read side repository. TeamId: {TeamId}", notification.TeamId);
			}
		}
	}
}
