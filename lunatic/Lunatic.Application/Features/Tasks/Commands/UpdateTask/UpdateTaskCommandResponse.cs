
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Responses;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTask {
	public class UpdateTaskCommandResponse : BaseResponse {
		public UpdateTaskCommandResponse() : base() { }

		public TaskDto Task { get; set; } = default!;
	}
}
