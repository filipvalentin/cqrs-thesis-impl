
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Teams.Payload;


namespace Lunatic.Application.Features.Teams.Commands.RemoveTeamMember {
    public class RemoveTeamMemberCommandResponse : BaseResponse {
        public RemoveTeamMemberCommandResponse() : base() { }

        public TeamDto Team { get; set; } = default!;
    }
}
