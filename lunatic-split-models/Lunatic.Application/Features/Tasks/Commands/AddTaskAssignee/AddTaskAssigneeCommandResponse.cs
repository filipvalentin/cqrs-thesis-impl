
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Commands.AddTaskAssignee {
    public class AddTaskAssigneeCommandResponse : BaseResponse {
        public AddTaskAssigneeCommandResponse() : base() { }

        public TaskDto Task { get; set; } = default!;
    }
}
