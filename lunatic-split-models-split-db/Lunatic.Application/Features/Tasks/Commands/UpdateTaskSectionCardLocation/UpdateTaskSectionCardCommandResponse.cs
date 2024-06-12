using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Responses;

namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskSectionCardLocation {
	public class UpdateTaskSectionCardCommandResponse : BaseResponse {
		public UpdateTaskSectionCardCommandResponse() : base() { }
		public TaskDto Task { get; set; } = default!;
	}
}
