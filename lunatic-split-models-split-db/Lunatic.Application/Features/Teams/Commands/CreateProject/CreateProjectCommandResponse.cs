using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Responses;

namespace Lunatic.Application.Features.Teams.Commands.CreateProject {
	public class CreateProjectCommandResponse : BaseResponse {
		public CreateProjectCommandResponse() : base() { }

		public ProjectDto Project { get; set; } = default!;
	}
}
