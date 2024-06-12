using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Responses;

namespace Lunatic.Application.Features.Projects.Commands.CreateTaskSectionCard {
	public class AddTaskSectionCardCommandResponse : BaseResponse {
		public AddTaskSectionCardCommandResponse() : base() { }
		public ProjectDto Project { get; set; } = default!;
	}
}

