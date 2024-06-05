using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Teams.Events {
	public class TeamCreatedDomainEventHandler : INotificationHandler<TeamCreatedDomainEvent> {
		private readonly ITeamReadSideRepository teamReadRepository;
		private readonly ITeamRepository teamWriteRepository;
		private readonly ILogger<TeamCreatedDomainEventHandler> logger;
		private readonly IMapper mapper;

		public TeamCreatedDomainEventHandler(ITeamReadSideRepository teamRepository,
			ITeamRepository teamWriteRepository, ILogger<TeamCreatedDomainEventHandler> logger, IMapper mapper) {
			this.teamReadRepository = teamRepository;
			this.teamWriteRepository = teamWriteRepository;
			this.logger = logger;
			this.mapper = mapper;
		}

		public async Task Handle(TeamCreatedDomainEvent notification, CancellationToken cancellationToken) {
			var teamResult = await teamWriteRepository.FindByIdAsync(notification.Id);

			if (!teamResult.IsSuccess) {
				logger.LogError("Team not found in write side repository. TeamId: {TeamId}", notification.Id);
				return;
			}

			var addResult = await teamReadRepository.AddAsync(mapper.Map<TeamReadModel>(teamResult.Value));

			if (!addResult.IsSuccess) {
				logger.LogError("Failed to add team to read side repository. TeamId: {TeamId}", notification.Id);
			}
		}
	}
}
