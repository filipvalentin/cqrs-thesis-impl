
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.RemoveTeamMember {
    public class RemoveTeamMemberCommand : IRequest<RemoveTeamMemberCommandResponse> {
        public Guid UserId { get; set; } = default!;
        public Guid TeamId { get; set; } = default!;
    }
}
