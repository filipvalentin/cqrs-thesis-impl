using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Responses;


namespace Lunatic.Application.Features.Projects.Commands.CreateProjectTask {
	public class CreateProjectTaskCommandResponse : BaseResponse {
		public CreateProjectTaskCommandResponse() : base() { }

		public TaskDto Task { get; set; } = default!;
	}
}
