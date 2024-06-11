
using AutoMapper;
using Lunatic.Application.Features.Teams.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, UpdateTeamCommandResponse> {
		private readonly ITeamRepository teamRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public UpdateTeamCommandHandler(ITeamRepository teamRepository, IPublisher publisher, IMapper mapper) {
			this.teamRepository = teamRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<UpdateTeamCommandResponse> Handle(UpdateTeamCommand request, CancellationToken cancellationToken) {
			//var validator = new UpdateTeamCommandValidator(teamRepository);
			//var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			//if (!validatorResult.IsValid) {
			//	return new UpdateTeamCommandResponse {
			//		Success = false,
			//		ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
			//	};
			//}

			var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
			if (!teamResult.IsSuccess) {
				return new UpdateTeamCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "Team not found" }
				};
			}

			teamResult.Value.Update(request.Name, request.Description);

			var dbTeamResult = await teamRepository.UpdateAsync(teamResult.Value);

			return new UpdateTeamCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(dbTeamResult.Value)
			};
		}
	}
}
