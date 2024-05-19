
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Projects.Payload;


namespace Lunatic.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateTeamProjectCommandResponse : BaseResponse
    {
        public UpdateTeamProjectCommandResponse() : base() { }

        public ProjectDto Project { get; set; } = default!;
    }
}
