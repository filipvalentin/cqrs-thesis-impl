using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Teams.Events {
	public class TeamDetailsUpdatedDomainEventHandler(
		ITeamReadSideRepository teamRepository,
		ILogger<TeamDetailsUpdatedDomainEventHandler> logger,
		IEventQueueService queueService,
		IMapper mapper) : INotificationHandler<TeamDetailsUpdatedDomainEvent> {

		private readonly ITeamReadSideRepository teamReadRepository = teamRepository;
		private readonly ILogger<TeamDetailsUpdatedDomainEventHandler> logger = logger;
		private readonly IEventQueueService queueService = queueService;
		private readonly IMapper mapper = mapper;

		public async Task Handle(TeamDetailsUpdatedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var updateResult = await teamReadRepository.UpdateAsync(domainEvent.Id, mapper.Map<TeamReadModel>(domainEvent));
			if (!updateResult.IsSuccess) {
				logger.LogError("Error from TeamReadSideRepository: {Error} when updating entity with {Id}", updateResult.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
