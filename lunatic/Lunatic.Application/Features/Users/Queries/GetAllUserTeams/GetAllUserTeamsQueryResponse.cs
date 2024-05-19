
using Lunatic.Application.Features.Teams.Payload;
using Lunatic.Application.Responses;


namespace Lunatic.Application.Features.Users.Queries.GetAllUserTeams {
    public class GetAllUserTeamsQueryResponse : BaseResponse {
        public GetAllUserTeamsQueryResponse() : base() { }
        public List<TeamDto> Teams { get; set; } = default!;
    }
}
