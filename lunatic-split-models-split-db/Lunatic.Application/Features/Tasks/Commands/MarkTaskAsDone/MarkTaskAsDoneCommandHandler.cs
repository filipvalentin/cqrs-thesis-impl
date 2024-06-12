using MediatR;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Task;
using AutoMapper;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus {
	public class MarkTaskAsDoneCommandHandler(
		ITaskRepository taskRepository,
		IPublisher publisher,
		IMapper mapper) : IRequestHandler<MarkTaskAsDoneCommand, MarkTaskAsDoneCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;

		public async Task<MarkTaskAsDoneCommandResponse> Handle(MarkTaskAsDoneCommand request, CancellationToken cancellationToken) {
			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);
			if (!taskResult.IsSuccess) {
				return new MarkTaskAsDoneCommandResponse {
					Success = false,
					Message = taskResult.Error
				};
			}

			taskResult.Value.SetStatus(Domain.Entities.TaskStatus.DONE);
			var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);
			if (!dbTaskResult.IsSuccess) {
				return new MarkTaskAsDoneCommandResponse {
					Success = false,
					Message = dbTaskResult.Error
				};
			}

			await publisher.Publish(new TaskCompletedDomainEvent(taskResult.Value.Id), cancellationToken);
			await publisher.Publish(mapper.Map<TaskUpdatedDomainEvent>(dbTaskResult.Value), cancellationToken);

			return new MarkTaskAsDoneCommandResponse { Success = true };
		}
	}
}
