using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.UpdateTask {
	public class UpdateTaskCommandHandler(
		ITaskRepository taskRepository,
		IMapper mapper,
		IPublisher publisher) : IRequestHandler<UpdateTaskCommand, UpdateTaskCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IMapper mapper = mapper;
		private readonly IPublisher publisher = publisher;

		public async Task<UpdateTaskCommandResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken) {
			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);
			if (!taskResult.IsSuccess) {
				return new UpdateTaskCommandResponse {
					Success = false,
					Message = taskResult.Error
				};
			}

			taskResult.Value.Update(request.Title, request.Description, request.Priority, request.PlannedStartDate, request.PlannedEndDate);
			taskResult.Value.UpdateLists(request.Tags, request.AssigneeIds);

			var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);
			if (!dbTaskResult.IsSuccess) {
				return new UpdateTaskCommandResponse {
					Success = false,
					Message = dbTaskResult.Error
				};
			}

			await publisher.Publish(mapper.Map<TaskUpdatedDomainEvent>(taskResult.Value), cancellationToken);

			return new UpdateTaskCommandResponse {
				Success = true,
				Task = mapper.Map<TaskDto>(dbTaskResult.Value)
			};
		}
	}
}
