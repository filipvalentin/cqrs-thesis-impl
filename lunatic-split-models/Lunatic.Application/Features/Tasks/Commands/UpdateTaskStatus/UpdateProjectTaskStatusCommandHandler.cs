
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Tasks.Payload;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus {
	public class UpdateProjectTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, UpdateTaskStatusCommandResponse>
    {
        private readonly ITaskRepository taskRepository;

        public UpdateProjectTaskStatusCommandHandler(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<UpdateTaskStatusCommandResponse> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateProjectTaskStatusCommandValidator(taskRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return new UpdateTaskStatusCommandResponse
                {
                    Success = false,
                    ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var taskResult = await taskRepository.FindByIdAsync(request.TaskId);

            taskResult.Value.SetStatus(request.Status);

            var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);

            return new UpdateTaskStatusCommandResponse
            {
                Success = true,
                Task = new TaskDto
                {
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
