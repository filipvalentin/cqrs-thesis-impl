
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Users.Payload;


namespace Lunatic.Application.Features.Teams.Queries.GetAllMembers {
    public class GetAllTeamMembersQueryResponse : BaseResponse {
        public GetAllTeamMembersQueryResponse() : base() {}

        public List<UserDto> Members { get; set; } = default!;
    }
}
