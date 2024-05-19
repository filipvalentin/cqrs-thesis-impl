
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Tasks.Payload;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.AddTaskTag {
    public class AddTaskTagCommandHandler : IRequestHandler<AddTaskTagCommand, AddTaskTagCommandResponse> {
        private readonly ITaskRepository taskRepository;

        public AddTaskTagCommandHandler(ITaskRepository taskRepository) {
            this.taskRepository = taskRepository;
        }

        public async Task<AddTaskTagCommandResponse> Handle(AddTaskTagCommand request, CancellationToken cancellationToken) {
            var validator = new AddTaskTagCommandValidator(taskRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if(!validatorResult.IsValid) {
                return new AddTaskTagCommandResponse {
                    Success = false,
                    ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var task = (await taskRepository.FindByIdAsync(request.TaskId)).Value;
            task.AddTag(request.Tag);
            var dbTaskResult = await taskRepository.UpdateAsync(task);

            return new AddTaskTagCommandResponse {
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
