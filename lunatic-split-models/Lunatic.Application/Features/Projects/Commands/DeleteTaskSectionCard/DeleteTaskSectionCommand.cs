using MediatR;


namespace Lunatic.Application.Features.Projects.Commands.DeleteTaskSectionCard {
	public class DeleteTaskSectionCommand : IRequest<DeleteTaskSectionCommandResponse> {
		public Guid ProjectId { get; set; } = default!;

		public string Section { get; set; } = default!;
	}
}
