
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Responses;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskSectionCardLocation {
	public class UpdateTaskSectionCommandResponse : BaseResponse {
		public UpdateTaskSectionCommandResponse() : base() { }

		public TaskDto Task { get; set; } = default!;
	}
}
