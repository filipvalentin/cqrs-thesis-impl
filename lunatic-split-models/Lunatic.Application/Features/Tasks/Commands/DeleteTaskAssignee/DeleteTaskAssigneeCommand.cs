
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.DeleteTaskAssignee {
    public class DeleteTaskAssigneeCommand : IRequest<DeleteTaskAssigneeCommandResponse> {
        public Guid UserId { get; set; } = default!;
        public Guid TaskId { get; set; } = default!;
    }
}
