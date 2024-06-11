
using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.UpdateTask {
	public class UpdateTaskCommandHandler(ITaskRepository taskRepository,
		IMapper mapper,
		IPublisher publisher) : IRequestHandler<UpdateTaskCommand, UpdateTaskCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IMapper mapper = mapper;
		private readonly IPublisher publisher = publisher;

		public async Task<UpdateTaskCommandResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken) {
			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);

			taskResult.Value.Update(request.Title, request.Description, request.Priority, request.PlannedStartDate, request.PlannedEndDate);
			taskResult.Value.UpdateLists(request.Tags, request.AssigneeIds);

			var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);

			await publisher.Publish(mapper.Map<TeamUpdatedDomainEvent>(taskResult.Value), cancellationToken);

			return new UpdateTaskCommandResponse {
				Success = true,
				Task = mapper.Map<TaskDto>(dbTaskResult.Value)
			};
		}
	}
}
