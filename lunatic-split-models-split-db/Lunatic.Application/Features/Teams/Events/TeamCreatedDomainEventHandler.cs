using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Utils;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Lunatic.Application.Features.Teams.Events {
	public class TeamCreatedDomainEventHandler(ITeamReadSideRepository teamRepository,
		IUserReadSideRepository userReadRepository,
		ITeamRepository teamWriteRepository,
		ILogger<TeamCreatedDomainEventHandler> logger,
		IEventQueueService queueService,
		IMapper mapper) : INotificationHandler<TeamCreatedDomainEvent> {

		private readonly ITeamReadSideRepository teamReadRepository = teamRepository;
		private readonly IUserReadSideRepository userReadRepository = userReadRepository;
		private readonly ITeamRepository teamWriteRepository = teamWriteRepository;
		private readonly ILogger<TeamCreatedDomainEventHandler> logger = logger;
		private readonly IEventQueueService queueService = queueService;
		private readonly IMapper mapper = mapper;

		public async Task Handle(TeamCreatedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var teamOwnerResult = await userReadRepository.FindByIdAsync(domainEvent.CreatedByUserId);
			if (!teamOwnerResult.IsSuccess) {
				logger.LogError("Error from UserReadSideRepository: {Error} when searching for {Id}", teamOwnerResult.Error, domainEvent.CreatedByUserId);
				queueService.Enqueue(domainEvent);
				return;
			}

			var teamOwner = teamOwnerResult.Value;
			teamOwner.TeamIds.AddIfNotExists(domainEvent.Id);
			var updateOwnerResult = await userReadRepository.UpdateAsync(teamOwner.Id, teamOwner);
			if (!updateOwnerResult.IsSuccess) {
				logger.LogError("Error from UserReadSideRepository: {Error} when updating entity with {Id}", updateOwnerResult.Error, teamOwner.Id);
				queueService.Enqueue(domainEvent);
				return;
			}

			var addResult = await teamReadRepository.AddAsync(mapper.Map<TeamReadModel>(domainEvent));
			if (!addResult.IsSuccess) {
				logger.LogError("Error from TeamReadSideRepository: {Error} when adding entity with {Id}", addResult.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
