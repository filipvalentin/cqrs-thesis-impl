using AutoMapper;
using Lunatic.Application.Features.Teams.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Team;
using Lunatic.Domain.Entities;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.CreateTeam {
	public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, CreateTeamCommandResponse> {
		private readonly ITeamRepository teamRepository;
		private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public CreateTeamCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository, IPublisher publisher, IMapper mapper) {
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<CreateTeamCommandResponse> Handle(CreateTeamCommand request, CancellationToken cancellationToken) {
			//var validator = new CreateTeamCommandValidator(userRepository);
			//var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			//if (!validatorResult.IsValid) {
			//	return new CreateTeamCommandResponse {
			//		Success = false,
			//		ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
			//	};
			//}

			var teamResult = Team.Create(request.UserId, request.Name, request.Description);
			if(!teamResult.IsSuccess) {
				return new CreateTeamCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { teamResult.Error }
				};
			}
			var team = teamResult.Value;
			team.AddMember(request.UserId);
			await teamRepository.AddAsync(team);

			var user = (await userRepository.FindByIdAsync(request.UserId)).Value;
			user.AddTeam(team.Id);
			await userRepository.UpdateAsync(user);

			await publisher.Publish(mapper.Map<TeamCreatedDomainEvent>(team), cancellationToken);

			return new CreateTeamCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(team)
			};
		}
	}
}
