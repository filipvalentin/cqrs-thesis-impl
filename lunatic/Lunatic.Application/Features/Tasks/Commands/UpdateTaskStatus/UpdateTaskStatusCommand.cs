using MediatR;
using TaskStatus = Lunatic.Domain.Entities.TaskStatus;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommand : IRequest<UpdateTaskStatusCommandResponse>
    {
        public Guid TaskId { get; set; } = default!;

        public TaskStatus Status { get; set; } = default!;
    }
}
