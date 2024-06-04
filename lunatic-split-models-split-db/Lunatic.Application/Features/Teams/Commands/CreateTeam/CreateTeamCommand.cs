
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.CreateTeam {
    public class CreateTeamCommand : IRequest<CreateTeamCommandResponse> {
        public Guid UserId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
