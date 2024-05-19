
using Lunatic.Application.Features.Teams.Payload;
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.CreateTeam {
	public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, CreateTeamCommandResponse> {
		private readonly ITeamRepository teamRepository;
		private readonly IUserRepository userRepository;

		public CreateTeamCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository) {
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
		}

		public async Task<CreateTeamCommandResponse> Handle(CreateTeamCommand request, CancellationToken cancellationToken) {
			var validator = new CreateTeamCommandValidator(userRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new CreateTeamCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var team = Team.Create(request.UserId, request.Name, request.Description).Value;

			team.AddMember(request.UserId);
			await teamRepository.AddAsync(team);
			var user = (await userRepository.FindByIdAsync(request.UserId)).Value;
			user.AddTeam(team.Id);
			await userRepository.UpdateAsync(user);

			return new CreateTeamCommandResponse {
				Success = true,
				Team = new TeamDto {
					TeamId = team.Id,

					Name = team.Name,
					Description = team.Description,

					MemberIds = team.MemberIds,
					ProjectIds = team.ProjectIds,
				}
			};
		}
	}
}
