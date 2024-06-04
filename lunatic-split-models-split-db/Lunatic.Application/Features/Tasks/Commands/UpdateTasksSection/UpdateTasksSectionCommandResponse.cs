
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Responses;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTasksSection {
	public class UpdateTasksSectionCommandResponse : BaseResponse {
		public UpdateTasksSectionCommandResponse() : base() { }

		public ProjectDto Project { get; set; } = default!;
	}
}
