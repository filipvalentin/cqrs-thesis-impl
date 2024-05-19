using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTasksSection {
	public class UpdateTaskSectionCommand : IRequest<UpdateTasksSectionCommandResponse> {
		public Guid ProjectId { get; set; } = default!;

		public string Section { get; set; } = default!;
		public string NewSection { get; set; } = default!;
	}
}
