using Lunatic.Application.Features.Teams.Payload;
using MediatR;
using Lunatic.Application.Persistence.WriteSide;
using AutoMapper;

namespace Lunatic.Application.Features.Teams.Commands.RemoveTeamMember {
	public class RemoveTeamMemberCommandHandler : IRequestHandler<RemoveTeamMemberCommand, RemoveTeamMemberCommandResponse> {
		private readonly ITeamRepository teamRepository;
		private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public RemoveTeamMemberCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository, 
			IPublisher publisher, IMapper mapper) {
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<RemoveTeamMemberCommandResponse> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken) {
			//var validator = new DeleteTeamMemberCommandValidator(userRepository, teamRepository);
			//var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			//if (!validatorResult.IsValid) {
			//	return new DeleteTeamMemberCommandResponse {
			//		Success = false,
			//		ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
			//	};
			//}

			var team = (await teamRepository.FindByIdAsync(request.TeamId)).Value;
			team.RemoveMember(request.UserId);
			var dbTeamResult = await teamRepository.UpdateAsync(team);

			return new RemoveTeamMemberCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(dbTeamResult.Value)
			};
		}
	}
}
