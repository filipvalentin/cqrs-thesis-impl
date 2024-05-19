
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTask {
	public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, UpdateTaskCommandResponse> {
		private readonly ITaskRepository taskRepository;

		public UpdateTaskCommandHandler(ITaskRepository taskRepository) {
			this.taskRepository = taskRepository;
		}

		public async Task<UpdateTaskCommandResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken) {
			var validator = new UpdateTaskCommandValidator(taskRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new UpdateTaskCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);

			taskResult.Value.Update(request.Title, request.Description, request.Priority, request.PlannedStartDate, request.PlannedEndDate);

			taskResult.Value.UpdateLists(request.Tags, request.AssigneeIds);

			var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);

			return new UpdateTaskCommandResponse {
				Success = true,
				Task = new TaskDto {
					CreatedByUserId = dbTaskResult.Value.CreatedByUserId,
					TaskId = dbTaskResult.Value.Id,
					ProjectId = dbTaskResult.Value.ProjectId,

					Section = dbTaskResult.Value.TaskSectionCard,

					Title = dbTaskResult.Value.Title,
					Description = dbTaskResult.Value.Description,
					Priority = dbTaskResult.Value.Priority,
					Status = dbTaskResult.Value.Status,

					Tags = dbTaskResult.Value.Tags,
					CommentIds = dbTaskResult.Value.CommentIds,
					AssigneeIds = dbTaskResult.Value.AssigneeIds,

					PlannedStartDate = dbTaskResult.Value.PlannedStartDate,
					PlannedEndDate = dbTaskResult.Value.PlannedEndDate,
					StartedDate = dbTaskResult.Value.StartedDate,
					EndedDate = dbTaskResult.Value.EndedDate,
				}
			};
		}
	}
}
