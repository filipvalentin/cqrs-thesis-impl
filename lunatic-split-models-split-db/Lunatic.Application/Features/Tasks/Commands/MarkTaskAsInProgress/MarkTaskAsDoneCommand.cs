using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus {
	public class MarkTaskAsInProgressCommand : IRequest<MarkTaskAsInProgressCommandResponse> {
		public Guid TaskId { get; set; } = default!;

	}
}
