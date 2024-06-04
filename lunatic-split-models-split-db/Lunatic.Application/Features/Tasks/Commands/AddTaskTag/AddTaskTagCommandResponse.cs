
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Commands.AddTaskTag {
    public class AddTaskTagCommandResponse : BaseResponse {
        public AddTaskTagCommandResponse() : base() { }

        public TaskDto Task { get; set; } = default!;
    }
}
