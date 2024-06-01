
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandResponse : BaseResponse
    {
        public UpdateTaskStatusCommandResponse() : base() { }

        public TaskDto Task { get; set; } = default!;
    }
}
