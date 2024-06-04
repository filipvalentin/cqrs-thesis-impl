using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.DeleteTeam
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, DeleteTeamCommandResponse> {
		private readonly IUserRepository userRepository;
		private readonly ITeamRepository teamRepository;

		public DeleteTeamCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository) {
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
		}

		public async Task<DeleteTeamCommandResponse> Handle(DeleteTeamCommand request, CancellationToken cancellationToken) {
			var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
			if (teamResult.IsSuccess) {
				foreach (var memberId in teamResult.Value.MemberIds) {
					var user = (await userRepository.FindByIdAsync(memberId)).Value;
					user.RemoveTeam(request.TeamId);
					await userRepository.UpdateAsync(user);
				}
			}
			var result = await teamRepository.DeleteAsync(request.TeamId);

			if (!result.IsSuccess) {
				return new DeleteTeamCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};

			}
			return new DeleteTeamCommandResponse {
				Success = true
			};
		}
	}
}
