﻿using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;

namespace Lunatic.Application.Features.Teams.Commands.DeleteTeam {
	public class DisbandTeamCommandHandler : IRequestHandler<DisbandTeamCommand, DisbandTeamCommandResponse> {
		private readonly IUserRepository userRepository;
		private readonly ITeamRepository teamRepository;
		private readonly IPublisher publisher;

		public DisbandTeamCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository, IPublisher publisher) {
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
			this.publisher = publisher;
		}

		public async Task<DisbandTeamCommandResponse> Handle(DisbandTeamCommand request, CancellationToken cancellationToken) {
			var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
			if (!teamResult.IsSuccess) {
				return new DisbandTeamCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { teamResult.Error }
				};
			}

			var team = teamResult.Value;

			foreach (var memberId in team.MemberIds) {
				var user = (await userRepository.FindByIdAsync(memberId)).Value;
				user.RemoveTeam(team.Id);
				await userRepository.UpdateAsync(user);
			}

			var result = await teamRepository.DeleteAsync(team.Id);

			if (!result.IsSuccess) {
				return new DisbandTeamCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}

			await publisher.Publish(new TeamDisbandedDomainEvent(team.Id, team.ProjectIds), cancellationToken);

			return new DisbandTeamCommandResponse {
				Success = true
			};
		}
	}
}