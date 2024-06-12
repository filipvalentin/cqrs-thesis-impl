using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.MarkTaskAsInProgress {
	public class MarkTaskAsInProgressCommand : IRequest<MarkTaskAsInProgressCommandResponse> {
		public Guid TaskId { get; set; } = default!;
	}
}
