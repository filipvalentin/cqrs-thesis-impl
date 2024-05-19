using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamProject
{
    public class DeleteTeamProjectCommand : IRequest<DeleteTeamProjectCommandResponse>
    {
        public Guid TeamId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
