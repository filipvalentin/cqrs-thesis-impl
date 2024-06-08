using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Responses;

namespace Lunatic.Application.Features.Projects.Commands.RenameTaskSection {
	public class RenameTaskSectionCardCommandResponse : BaseResponse {
		public RenameTaskSectionCardCommandResponse() : base() { }
		public ProjectDto Project { get; set; } = default!;
	}
}
