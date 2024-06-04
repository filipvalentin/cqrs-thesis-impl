using MediatR;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Task;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus {
	public class MarkTaskAsDoneCommandHandler : IRequestHandler<MarkTaskAsDoneCommand, MarkTaskAsDoneCommandResponse> {
		private readonly ITaskRepository taskRepository;
		private readonly IPublisher publisher;

		public MarkTaskAsDoneCommandHandler(ITaskRepository taskRepository, IPublisher publisher) {
			this.taskRepository = taskRepository;
			this.publisher = publisher;
		}

		public async Task<MarkTaskAsDoneCommandResponse> Handle(MarkTaskAsDoneCommand request, CancellationToken cancellationToken) {

			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);

			if (!taskResult.IsSuccess) {
				return new MarkTaskAsDoneCommandResponse {
					Success = false,
					Message = "Task not found"
				};
			}

			taskResult.Value.SetStatus(Domain.Entities.TaskStatus.DONE);

			var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);

			await publisher.Publish(new TaskCompletedDomainEvent(taskResult.Value.Id), cancellationToken);

			return new MarkTaskAsDoneCommandResponse { Success = true };
		}
	}
}
