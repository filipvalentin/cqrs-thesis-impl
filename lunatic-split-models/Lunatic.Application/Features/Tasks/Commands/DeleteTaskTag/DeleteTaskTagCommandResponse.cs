
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Commands.DeleteTaskTag {
    public class DeleteTaskTagCommandResponse : BaseResponse {
        public DeleteTaskTagCommandResponse() : base() { }

        public TaskDto Task { get; set; } = default!;
    }
}
