
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.AddTaskTag {
    public class AddTaskTagCommand : IRequest<AddTaskTagCommandResponse> {
        public Guid TaskId { get; set; } = default!;
        public string Tag { get; set; } = default!;
    }
}
