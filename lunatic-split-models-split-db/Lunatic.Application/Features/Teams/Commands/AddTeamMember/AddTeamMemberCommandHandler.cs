using Lunatic.Application.Features.Teams.Payload;
using MediatR;
using Lunatic.Application.Persistence.WriteSide;
using AutoMapper;
using Lunatic.Domain.DomainEvents.Team;

namespace Lunatic.Application.Features.Teams.Commands.AddTeamMember {
	public class AddTeamMemberCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository, IMapper mapper, IPublisher publisher) : IRequestHandler<AddTeamMemberCommand, AddTeamMemberCommandResponse> {
		private readonly ITeamRepository teamRepository = teamRepository;
		private readonly IUserRepository userRepository = userRepository;
		private readonly IMapper mapper = mapper;
		private readonly IPublisher publisher = publisher;

		public async Task<AddTeamMemberCommandResponse> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken) {

			var team = (await teamRepository.FindByIdAsync(request.TeamId)).Value;
			team.AddMember(request.UserId);

			var user = (await userRepository.FindByIdAsync(request.UserId)).Value;
			user.AddTeam(request.TeamId);
			await userRepository.UpdateAsync(user);

			var dbTeamResult = await teamRepository.UpdateAsync(team);

			await publisher.Publish(new TeamMemberAddedDomainEvent(team.Id, request.UserId), cancellationToken);

			return new AddTeamMemberCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(dbTeamResult.Value)
			};
		}
	}
}
