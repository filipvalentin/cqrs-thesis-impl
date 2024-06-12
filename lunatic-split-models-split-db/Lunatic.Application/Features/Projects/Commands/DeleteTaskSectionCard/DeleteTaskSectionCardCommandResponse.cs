using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Responses;

namespace Lunatic.Application.Features.Projects.Commands.DeleteTaskSectionCard {
	public class DeleteTaskSectionCardCommandResponse : BaseResponse {
		public DeleteTaskSectionCardCommandResponse() : base() { }
		public ProjectDto Project { get; set; } = default!;
	}
}

