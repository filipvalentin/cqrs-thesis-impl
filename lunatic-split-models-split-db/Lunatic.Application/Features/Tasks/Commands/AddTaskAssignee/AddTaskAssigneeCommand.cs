
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.AddTaskAssignee {
    public class AddTaskAssigneeCommand : IRequest<AddTaskAssigneeCommandResponse> {
        public Guid UserId { get; set; } = default!;
        public Guid TaskId { get; set; } = default!;
    }
}
