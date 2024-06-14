using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Teams.Events {
	public class TeamDetailsUpdatedDomainEventHandler(
		ITeamReadSideRepository teamReadRepository,
		IMapper mapper,
		IEventQueueService queueService,
		ILogger<TeamDetailsUpdatedDomainEventHandler> logger) : INotificationHandler<TeamDetailsUpdatedDomainEvent> {

		private readonly ITeamReadSideRepository teamReadRepository = teamReadRepository;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;
		private readonly ILogger<TeamDetailsUpdatedDomainEventHandler> logger = logger;

		public async Task Handle(TeamDetailsUpdatedDomainEvent notification, CancellationToken cancellationToken) {
			var updateResult = await teamReadRepository.UpdateAsync(notification.Id, mapper.Map<TeamReadModel>(notification));
			if (!updateResult.IsSuccess) {
				logger.LogError("Error from TeamReadSideRepository: {Error} when updating entity with {Id}", updateResult.Error, notification.Id);
				queueService.Enqueue(notification);
			}
		}
	}
}
