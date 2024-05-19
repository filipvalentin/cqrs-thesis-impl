using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.AddTeamProject
{
    public class CreateTeamProjectCommand : IRequest<CreateTeamProjectCommandResponse>
    {
        public Guid UserId { get; set; } = default!;
        public Guid TeamId { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
