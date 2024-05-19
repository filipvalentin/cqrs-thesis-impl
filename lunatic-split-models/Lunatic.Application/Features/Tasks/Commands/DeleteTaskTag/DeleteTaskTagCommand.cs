
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.DeleteTaskTag {
    public class DeleteTaskTagCommand : IRequest<DeleteTaskTagCommandResponse> {
        public Guid TaskId { get; set; } = default!;
        public string Tag { get; set; } = default!;
    }
}
