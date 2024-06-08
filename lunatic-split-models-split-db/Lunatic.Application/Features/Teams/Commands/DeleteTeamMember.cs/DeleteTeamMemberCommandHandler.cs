using Lunatic.Application.Features.Teams.Payload;
using MediatR;
using Lunatic.Application.Persistence.WriteSide;
using AutoMapper;

namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamMember.cs {
	public class DeleteTeamMemberCommandHandler : IRequestHandler<DeleteTeamMemberCommand, DeleteTeamMemberCommandResponse> {
		private readonly ITeamRepository teamRepository;
		private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public DeleteTeamMemberCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository, 
			IPublisher publisher, IMapper mapper) {
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<DeleteTeamMemberCommandResponse> Handle(DeleteTeamMemberCommand request, CancellationToken cancellationToken) {
			var validator = new DeleteTeamMemberCommandValidator(userRepository, teamRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new DeleteTeamMemberCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var team = (await teamRepository.FindByIdAsync(request.TeamId)).Value;
			team.RemoveMember(request.UserId);
			var dbTeamResult = await teamRepository.UpdateAsync(team);

			return new DeleteTeamMemberCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(dbTeamResult.Value)
			};
		}
	}
}
