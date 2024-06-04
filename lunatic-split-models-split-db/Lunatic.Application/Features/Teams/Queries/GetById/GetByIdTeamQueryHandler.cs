using Lunatic.Application.Features.Teams.Payload;
using Lunatic.Application.Persistence.ReadSide;
using MediatR;


namespace Lunatic.Application.Features.Teams.Queries.GetById {
	public class GetByIdTeamQueryHandler : IRequestHandler<GetByIdTeamQuery, GetByIdTeamQueryResponse> {
		private readonly ITeamReadSideRepository teamRepository;

		public GetByIdTeamQueryHandler(ITeamReadSideRepository teamRepository) {
			this.teamRepository = teamRepository;
		}

		public async Task<GetByIdTeamQueryResponse> Handle(GetByIdTeamQuery request, CancellationToken cancellationToken) {
            var result = await teamRepository.GetByIdAsync(request.TeamId);

            if(!result.IsSuccess) {
                return new GetByIdTeamQueryResponse {
					Success = false,
                    ValidationErrors = new List<string> { result.Error }
                };
            }

			var teamReadModel = result.Value;

			return new GetByIdTeamQueryResponse {
                Success = true,
                Team = new TeamDto {
                    TeamId = teamReadModel.Id,
                    OwnerId = teamReadModel.CreatedByUserId,
                    Name = teamReadModel.Name,
                    Description = teamReadModel.Description,
                    MemberIds = teamReadModel.MemberIds,
                    ProjectIds = teamReadModel.ProjectIds,
                }
            };
        }
    }
}
