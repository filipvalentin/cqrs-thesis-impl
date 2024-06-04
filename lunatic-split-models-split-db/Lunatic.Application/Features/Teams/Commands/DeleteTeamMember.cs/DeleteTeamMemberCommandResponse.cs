
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Teams.Payload;


namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamMember.cs {
    public class DeleteTeamMemberCommandResponse : BaseResponse {
        public DeleteTeamMemberCommandResponse() : base() { }

        public TeamDto Team { get; set; } = default!;
    }
}
