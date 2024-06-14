using Lunatic.Application.Features.Teams.Payload;
using MediatR;
using Lunatic.Application.Persistence.WriteSide;
using AutoMapper;
using Lunatic.Domain.DomainEvents.Team;
using Lunatic.Domain.DomainEvents.User;

namespace Lunatic.Application.Features.Teams.Commands.RemoveTeamMember {
	public class RemoveTeamMemberCommandHandler(
		ITeamRepository teamRepository,
		IUserRepository userRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<RemoveTeamMemberCommand, RemoveTeamMemberCommandResponse> {

		private readonly ITeamRepository teamRepository = teamRepository;
		private readonly IUserRepository userRepository = userRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<RemoveTeamMemberCommandResponse> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken) {

			var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
			if (!teamResult.IsSuccess) {
				return new RemoveTeamMemberCommandResponse {
					Success = false,
					Message = teamResult.Error
				};
			}
			var team = teamResult.Value;
			team.RemoveMember(request.UserId);

			var userResult = await userRepository.FindByIdAsync(request.UserId);
			if (!userResult.IsSuccess) {
				return new RemoveTeamMemberCommandResponse {
					Success = false,
					Message = userResult.Error
				};
			}
			var user = userResult.Value;
			user.RemoveTeam(request.TeamId);
			var userUpdatedResult = await userRepository.UpdateAsync(user);
			if (!userUpdatedResult.IsSuccess) {
				return new RemoveTeamMemberCommandResponse {
					Success = false,
					Message = userUpdatedResult.Error
				};
			}

			var teamUpdatedResult = await teamRepository.UpdateAsync(team);
			if (!teamUpdatedResult.IsSuccess) {
				return new RemoveTeamMemberCommandResponse {
					Success = false,
					Message = teamUpdatedResult.Error
				};
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<TeamDetailsUpdatedDomainEvent>(team), cancellationToken);
			await publisher.Publish(mapper.Map<UserUpdatedDomainEvent>(user), cancellationToken);

			return new RemoveTeamMemberCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(team)
			};
		}
	}
}
