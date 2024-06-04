using Lunatic.Application.Features.Tasks.Payload;
using MediatR;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Tasks.Commands.DeleteTaskTag
{
    public class DeleteTaskTagCommandHandler : IRequestHandler<DeleteTaskTagCommand, DeleteTaskTagCommandResponse> {
        private readonly ITaskRepository taskRepository;

        public DeleteTaskTagCommandHandler(ITaskRepository taskRepository) {
            this.taskRepository = taskRepository;
        }

        public async Task<DeleteTaskTagCommandResponse> Handle(DeleteTaskTagCommand request, CancellationToken cancellationToken) {
            var validator = new DeleteTaskTagCommandValidator(taskRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if(!validatorResult.IsValid) {
                return new DeleteTaskTagCommandResponse {
                    Success = false,
                    ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var task = (await taskRepository.FindByIdAsync(request.TaskId)).Value;
            task.RemoveTag(request.Tag);
            var dbTaskResult = await taskRepository.UpdateAsync(task);

            return new DeleteTaskTagCommandResponse {
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
