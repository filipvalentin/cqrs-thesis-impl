using MediatR;
using TaskStatus = Lunatic.Domain.Entities.TaskStatus;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus {
	public class MarkTaskAsDoneCommand : IRequest<MarkTaskAsDoneCommandResponse> {
		public Guid TaskId { get; set; } = default!;

	}
}
