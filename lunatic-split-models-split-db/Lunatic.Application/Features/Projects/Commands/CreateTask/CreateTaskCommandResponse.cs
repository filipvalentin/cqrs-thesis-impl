using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Responses;

namespace Lunatic.Application.Features.Projects.Commands.CreateTask {
	public class CreateTaskCommandResponse : BaseResponse {
		public CreateTaskCommandResponse() : base() { }
		public TaskDto Task { get; set; } = default!;
	}
}
