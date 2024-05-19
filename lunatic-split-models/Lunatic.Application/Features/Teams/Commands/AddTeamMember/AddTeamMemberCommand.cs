
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.AddTeamMember {
    public class AddTeamMemberCommand : IRequest<AddTeamMemberCommandResponse> {
        public Guid UserId { get; set; } = default!;
        public Guid TeamId { get; set; } = default!;
    }
}
