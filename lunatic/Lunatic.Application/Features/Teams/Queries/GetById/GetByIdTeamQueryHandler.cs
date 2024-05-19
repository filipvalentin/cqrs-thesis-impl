
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Teams.Payload;
using MediatR;


namespace Lunatic.Application.Features.Teams.Queries.GetById {
    public class GetByIdTeamQueryHandler : IRequestHandler<GetByIdTeamQuery, GetByIdTeamQueryResponse> {
        private readonly ITeamRepository teamRepository;

        public GetByIdTeamQueryHandler(ITeamRepository teamRepository) {
            this.teamRepository = teamRepository;
        }

        public async Task<GetByIdTeamQueryResponse> Handle(GetByIdTeamQuery request, CancellationToken cancellationToken) {
            var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
            if(!teamResult.IsSuccess) {
                return new GetByIdTeamQueryResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Team not found" }
                };
            }

            return new GetByIdTeamQueryResponse {
                Success = true,
                Team = new TeamDto {
                    TeamId = teamResult.Value.Id,
                    OwnerId = teamResult.Value.CreatedByUserId,
                    Name = teamResult.Value.Name,
                    Description = teamResult.Value.Description,

                    MemberIds = teamResult.Value.MemberIds,
                    ProjectIds = teamResult.Value.ProjectIds,
                }
            };
        }
    }
}
