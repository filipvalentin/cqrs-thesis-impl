using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.CreateTaskSectionCard {
	public class AddTaskSectionCardCommand : IRequest<AddTaskSectionCardCommandResponse> {
		public Guid ProjectId { get; set; } = default!;
		public string Section { get; set; } = default!;
	}
}
