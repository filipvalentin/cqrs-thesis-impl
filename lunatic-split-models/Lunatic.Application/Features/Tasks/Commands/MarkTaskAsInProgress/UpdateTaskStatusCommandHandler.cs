
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Tasks.Payload;
using MediatR;
using Lunatic.Domain.DomainEvents;
using Lunatic.Domain.Entities;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus {
	public class MarkTaskAsInProgressCommandHandler : IRequestHandler<MarkTaskAsInProgressCommand, MarkTaskAsInProgressCommandResponse> {
		private readonly ITaskRepository taskRepository;
		private readonly IPublisher publisher;

		public MarkTaskAsInProgressCommandHandler(ITaskRepository taskRepository, IPublisher publisher) {
			this.taskRepository = taskRepository;
			this.publisher = publisher;
		}

		public async Task<MarkTaskAsInProgressCommandResponse> Handle(MarkTaskAsInProgressCommand request, CancellationToken cancellationToken) {

			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);

			if (!taskResult.IsSuccess) {
				return new MarkTaskAsInProgressCommandResponse {
					Success = false,
					Message = "Task not found"
				};
			}

			taskResult.Value.SetStatus(Domain.Entities.TaskStatus.IN_PROGRESS);

			var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);

			//await publisher.Publish(new TaskCompletedDomainEvent(taskResult.Value.Id), cancellationToken);

			return new MarkTaskAsInProgressCommandResponse { Success = true };
		}
	}
}
