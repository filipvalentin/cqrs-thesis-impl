
using Lunatic.Domain.Entities;
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Teams.Payload;
using MediatR;


namespace Lunatic.Application.Features.Users.Queries.GetAllUserTeams {
    public class GetAllUserTeamsQueryHandler : IRequestHandler<GetAllUserTeamsQuery, GetAllUserTeamsQueryResponse> {
        private readonly IUserRepository userRepository;

        private readonly ITeamRepository teamRepository;

        public GetAllUserTeamsQueryHandler(IUserRepository userRepository, ITeamRepository teamRepository) {
            this.userRepository = userRepository;
            this.teamRepository = teamRepository;
        }

        public async Task<GetAllUserTeamsQueryResponse> Handle(GetAllUserTeamsQuery request, CancellationToken cancellationToken) {
            var userResult = await userRepository.FindByIdAsync(request.UserId);
            if(!userResult.IsSuccess) {
                return new GetAllUserTeamsQueryResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "User not found" }
                };
            }

            GetAllUserTeamsQueryResponse response = new GetAllUserTeamsQueryResponse();
            var teamIds = userResult.Value.TeamIds;
            var teams = new List<Team>();
            foreach (var teamId in teamIds) {
                var team = (await teamRepository.FindByIdAsync(teamId)).Value;
                teams.Add(team);
            }

            response.Teams = teams.Select(team => new TeamDto {
                TeamId = team.Id,
				OwnerId = team.CreatedByUserId,
                Name = team.Name,
                Description = team.Description,

                MemberIds = team.MemberIds,
                ProjectIds = team.ProjectIds,
            }).ToList();
            return response;
        }
    }
}
