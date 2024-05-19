using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskSectionCardLocation {
	public class UpdateTaskSectionLocationCommand : IRequest<UpdateTaskSectionCommandResponse> {
		public Guid TaskId { get; set; } = default!;

		public string Section { get; set; } = default!;
	}
}
