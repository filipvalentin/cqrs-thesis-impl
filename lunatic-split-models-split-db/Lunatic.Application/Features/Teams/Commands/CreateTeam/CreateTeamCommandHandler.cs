using AutoMapper;
using Lunatic.Application.Features.Teams.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Team;
using Lunatic.Domain.DomainEvents.User;
using Lunatic.Domain.Entities;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.CreateTeam {
	public class CreateTeamCommandHandler(
		ITeamRepository teamRepository,
		IUserRepository userRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<CreateTeamCommand, CreateTeamCommandResponse> {

		private readonly ITeamRepository teamRepository = teamRepository;
		private readonly IUserRepository userRepository = userRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<CreateTeamCommandResponse> Handle(CreateTeamCommand request, CancellationToken cancellationToken) {

			var teamResult = Team.Create(request.UserId, request.Name, request.Description);
			if (!teamResult.IsSuccess) {
				return new CreateTeamCommandResponse {
					Success = false,
					Message = teamResult.Error
				};
			}
			var team = teamResult.Value;
			team.AddMember(request.UserId);
			var teamCreadedResult = await teamRepository.AddAsync(team);
			if (!teamCreadedResult.IsSuccess) {
				return new CreateTeamCommandResponse {
					Success = false,
					Message = teamCreadedResult.Error
				};
			}

			var user = (await userRepository.FindByIdAsync(request.UserId)).Value;
			user.AddTeam(team.Id);
			await userRepository.UpdateAsync(user);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<TeamCreatedDomainEvent>(team), cancellationToken);
			await publisher.Publish(mapper.Map<UserUpdatedDomainEvent>(user), cancellationToken);

			return new CreateTeamCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(team)
			};
		}
	}
}
