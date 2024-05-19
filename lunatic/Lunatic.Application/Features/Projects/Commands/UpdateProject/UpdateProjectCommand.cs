using MediatR;


namespace Lunatic.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateTeamProjectCommand : IRequest<UpdateTeamProjectCommandResponse>
    {
        public Guid ProjectId { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
