
using AutoMapper;
using Lunatic.Application.Features.Teams.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;

namespace Lunatic.Application.Features.Teams.Commands.UpdateTeam {
	public class UpdateTeamCommandHandler(
		ITeamRepository teamRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<UpdateTeamCommand, UpdateTeamCommandResponse> {

		private readonly ITeamRepository teamRepository = teamRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<UpdateTeamCommandResponse> Handle(UpdateTeamCommand request, CancellationToken cancellationToken) {

			var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
			if (!teamResult.IsSuccess) {
				return new UpdateTeamCommandResponse {
					Success = false,
					Message = teamResult.Error
				};
			}

			teamResult.Value.Update(request.Name, request.Description);

			var dbTeamResult = await teamRepository.UpdateAsync(teamResult.Value);
			if (!dbTeamResult.IsSuccess) {
				return new UpdateTeamCommandResponse {
					Success = false,
					Message = dbTeamResult.Error
				};
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<TeamUpdatedDomainEvent>(teamResult.Value), cancellationToken);

			return new UpdateTeamCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(dbTeamResult.Value)
			};
		}
	}
}
