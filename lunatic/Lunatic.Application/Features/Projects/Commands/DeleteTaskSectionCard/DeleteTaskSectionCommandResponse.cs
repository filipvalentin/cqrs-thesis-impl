
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Responses;


namespace Lunatic.Application.Features.Projects.Commands.DeleteTaskSectionCard {
	public class DeleteTaskSectionCommandResponse : BaseResponse {
		public DeleteTaskSectionCommandResponse() : base() { }

		public ProjectDto Project { get; set; } = default!;
	}
}

