using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.RenameTaskSection {
	public class RenameTaskSectionCardCommand : IRequest<RenameTaskSectionCardCommandResponse> {
		public Guid ProjectId { get; set; } = default!;
		public string Section { get; set; } = default!;
		public string NewSection { get; set; } = default!;
	}
}
