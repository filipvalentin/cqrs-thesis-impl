
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskSectionCardLocation {
	public class UpdateProjectTaskSectionCommandHandler : IRequestHandler<UpdateTaskSectionLocationCommand, UpdateTaskSectionCommandResponse> {
		private readonly ITaskRepository taskRepository;

		private readonly IProjectRepository projectRepository;

		public UpdateProjectTaskSectionCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository) {
			this.taskRepository = taskRepository;
			this.projectRepository = projectRepository;
		}

		public async Task<UpdateTaskSectionCommandResponse> Handle(UpdateTaskSectionLocationCommand request, CancellationToken cancellationToken) {
			var validator = new UpdateTaskSectionCommandValidator(taskRepository, projectRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new UpdateTaskSectionCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);

			taskResult.Value.SetSection(request.Section);

			var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);

			return new UpdateTaskSectionCommandResponse {
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
