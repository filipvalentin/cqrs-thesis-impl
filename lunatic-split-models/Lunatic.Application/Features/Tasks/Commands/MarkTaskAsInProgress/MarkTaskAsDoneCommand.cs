using MediatR;
using TaskStatus = Lunatic.Domain.Entities.TaskStatus;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus {
	public class MarkTaskAsInProgressCommand : IRequest<MarkTaskAsInProgressCommandResponse> {
		public Guid TaskId { get; set; } = default!;

	}
}
