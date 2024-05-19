using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Responses;


namespace Lunatic.Application.Features.Teams.Commands.AddTeamProject
{
    public class CreateTeamProjectCommandResponse : BaseResponse
    {
        public CreateTeamProjectCommandResponse() : base() { }

        public ProjectDto Project { get; set; } = default!;
    }
}
