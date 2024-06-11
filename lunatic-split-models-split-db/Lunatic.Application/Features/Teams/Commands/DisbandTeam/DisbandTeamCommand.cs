using MediatR;

namespace Lunatic.Application.Features.Teams.Commands.DeleteTeam {
    public class DisbandTeamCommand : IRequest<DisbandTeamCommandResponse> {
        public Guid TeamId { get; set; }
    }
}
