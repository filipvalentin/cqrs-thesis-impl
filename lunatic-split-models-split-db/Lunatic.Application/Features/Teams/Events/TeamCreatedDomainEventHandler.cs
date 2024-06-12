using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Teams.Events {
	public class TeamCreatedDomainEventHandler(
		ITeamReadSideRepository teamRepository,
		ILogger<TeamCreatedDomainEventHandler> logger,
		IEventQueueService queueService,
		IMapper mapper) : INotificationHandler<TeamCreatedDomainEvent> {

		private readonly ITeamReadSideRepository teamReadRepository = teamRepository;
		private readonly ILogger<TeamCreatedDomainEventHandler> logger = logger;
		private readonly IEventQueueService queueService = queueService;
		private readonly IMapper mapper = mapper;

		public async Task Handle(TeamCreatedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var teamReadModel = mapper.Map<TeamReadModel>(domainEvent);
			teamReadModel.MemberIds.Add(domainEvent.CreatedByUserId);
			var addResult = await teamReadRepository.AddAsync(teamReadModel);
			if (!addResult.IsSuccess) {
				logger.LogError("Error from TeamReadSideRepository: {Error} when adding entity with {Id}", addResult.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
