using AutoMapper;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Team;
using Lunatic.Domain.DomainEvents.User;
using MediatR;

namespace Lunatic.Application.Features.Teams.Commands.DeleteTeam {
	public class DisbandTeamCommandHandler(
		ITeamRepository teamRepository,
		IUserRepository userRepository, 
		IPublisher publisher, 
		IMapper mapper) : IRequestHandler<DisbandTeamCommand, DisbandTeamCommandResponse> {

		private readonly IUserRepository userRepository = userRepository;
		private readonly ITeamRepository teamRepository = teamRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;

		public async Task<DisbandTeamCommandResponse> Handle(DisbandTeamCommand request, CancellationToken cancellationToken) {
			var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
			if (!teamResult.IsSuccess) {
				return new DisbandTeamCommandResponse {
					Success = false,
					Message = teamResult.Error
				};
			}
			var team = teamResult.Value;

			foreach (var memberId in team.MemberIds) {
				var userResult = await userRepository.FindByIdAsync(memberId);
				if (!userResult.IsSuccess) {
					return new DisbandTeamCommandResponse {
						Success = false,
						Message = userResult.Error
					};
				}
				var user = userResult.Value;
				user.RemoveTeam(team.Id);
				var userUpdatedResult = await userRepository.UpdateAsync(user);
				if (!userUpdatedResult.IsSuccess) {
					return new DisbandTeamCommandResponse {
						Success = false,
						Message = userUpdatedResult.Error
					};
				}
				await publisher.Publish(mapper.Map<UserUpdatedDomainEvent>(user), cancellationToken);
			}

			var result = await teamRepository.DeleteAsync(team.Id);
			if (!result.IsSuccess) {
				return new DisbandTeamCommandResponse {
					Success = false,
					Message = result.Error
				};
			}

			/*side-effects are handled in the domain event in cascade*/
			await publisher.Publish(new TeamDisbandedDomainEvent(team.Id, team.ProjectIds), cancellationToken);

			return new DisbandTeamCommandResponse {
				Success = true
			};
		}
	}
}
