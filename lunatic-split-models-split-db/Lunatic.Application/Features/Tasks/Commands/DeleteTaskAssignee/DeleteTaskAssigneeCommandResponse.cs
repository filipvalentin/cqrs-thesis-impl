
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Commands.DeleteTaskAssignee {
    public class DeleteTaskAssigneeCommandResponse : BaseResponse {
        public DeleteTaskAssigneeCommandResponse() : base() { }

        public TaskDto Task { get; set; } = default!;
    }
}
