using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Teams.Events {
	public class TeamMemberRemovedDomainEventHandler(ITeamReadSideRepository teamReadRepository,
		IUserReadSideRepository userReadRepository,
		ILogger<TeamMemberRemovedDomainEventHandler> logger,
		IEventQueueService queueService) : INotificationHandler<TeamMemberRemovedDomainEvent> {

		private readonly ITeamReadSideRepository teamReadRepository = teamReadRepository;
		private readonly IUserReadSideRepository userReadRepository = userReadRepository;
		private readonly ILogger<TeamMemberRemovedDomainEventHandler> logger = logger;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(TeamMemberRemovedDomainEvent notification, CancellationToken cancellationToken) {
			var teamResult = await teamReadRepository.FindByIdAsync(notification.Id);
			if (!teamResult.IsSuccess) {
				logger.LogError("Error from TeamReadSideRepository: {Error} when searching for {Id}", teamResult.Error, notification.Id);
				queueService.Enqueue(notification);
				return;
			}

			var userResult = await userReadRepository.FindByIdAsync(notification.MemberId);
			if (!userResult.IsSuccess) {
				logger.LogError("Error from UserReadSideRepository: {Error} when searching for {Id}", userResult.Error, notification.MemberId);
				queueService.Enqueue(notification);
				return;
			}

			var team = teamResult.Value;
			team.MemberIds.Remove(notification.MemberId);

			var user = userResult.Value;
			user.TeamIds.Remove(notification.Id);

			var updateUserResult = await userReadRepository.UpdateAsync(user.Id, user);
			if (!updateUserResult.IsSuccess) {
				logger.LogError("Error from UserReadSideRepository: {Error} when updating entity with {Id}", updateUserResult.Error, user.Id);
				queueService.Enqueue(notification);
				return;
			}

			var updateTeamResult = await teamReadRepository.UpdateAsync(team.Id, team);
			if (!updateTeamResult.IsSuccess) {
				logger.LogError("Error from TeamReadSideRepository: {Error} when updating entity with {Id}", updateTeamResult.Error, team.Id);
				queueService.Enqueue(notification);
			}
		}
	}
}
