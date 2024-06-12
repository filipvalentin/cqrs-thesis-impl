using Lunatic.Application.Features.Teams.Payload;
using MediatR;
using Lunatic.Application.Persistence.WriteSide;
using AutoMapper;
using Lunatic.Domain.DomainEvents.Team;
using Lunatic.Domain.DomainEvents.User;

namespace Lunatic.Application.Features.Teams.Commands.AddTeamMember {
	public class AddTeamMemberCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository, IMapper mapper, IPublisher publisher) : IRequestHandler<AddTeamMemberCommand, AddTeamMemberCommandResponse> {
		private readonly ITeamRepository teamRepository = teamRepository;
		private readonly IUserRepository userRepository = userRepository;
		private readonly IMapper mapper = mapper;
		private readonly IPublisher publisher = publisher;

		public async Task<AddTeamMemberCommandResponse> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken) {

			var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
			if (!teamResult.IsSuccess) {
				return new AddTeamMemberCommandResponse {
					Success = false,
					Message = teamResult.Error
				};
			}
			var team = teamResult.Value;
			team.AddMember(request.UserId);

			var userResult = await userRepository.FindByIdAsync(request.UserId);
			if(!userResult.IsSuccess) {
				return new AddTeamMemberCommandResponse {
					Success = false,
					Message = userResult.Error
				};
			}
			var user = userResult.Value;
			user.AddTeam(request.TeamId);
			var userUpdatedResult = await userRepository.UpdateAsync(user);
			if (!userUpdatedResult.IsSuccess) {
				return new AddTeamMemberCommandResponse {
					Success = false,
					Message = userUpdatedResult.Error
				};
			}		

			var teamUpdatedResult = await teamRepository.UpdateAsync(team);
			if (!teamUpdatedResult.IsSuccess) {
				return new AddTeamMemberCommandResponse {
					Success = false,
					Message = teamUpdatedResult.Error
				};
			}

			await publisher.Publish(mapper.Map<TeamUpdatedDomainEvent>(team), cancellationToken);
			await publisher.Publish(mapper.Map<UserUpdatedDomainEvent>(user), cancellationToken);

			return new AddTeamMemberCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(team)
			};
		}
	}
}
