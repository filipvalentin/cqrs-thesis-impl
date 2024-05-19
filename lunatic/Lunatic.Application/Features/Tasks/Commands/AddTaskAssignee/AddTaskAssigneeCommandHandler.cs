
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Tasks.Payload;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.AddTaskAssignee {
    public class AddTaskAssigneeCommandHandler : IRequestHandler<AddTaskAssigneeCommand, AddTaskAssigneeCommandResponse> {
        private readonly ITaskRepository taskRepository;

        private readonly IUserRepository userRepository;

        public AddTaskAssigneeCommandHandler(ITaskRepository taskRepository, IUserRepository userRepository) {
            this.taskRepository = taskRepository;
            this.userRepository = userRepository;
        }

        public async Task<AddTaskAssigneeCommandResponse> Handle(AddTaskAssigneeCommand request, CancellationToken cancellationToken) {
            var validator = new AddTaskAssigneeCommandValidator(userRepository, taskRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if(!validatorResult.IsValid) {
                return new AddTaskAssigneeCommandResponse {
                    Success = false,
                    ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var task = (await taskRepository.FindByIdAsync(request.TaskId)).Value;
            task.AddAssignee(request.UserId);
            var dbTaskResult = await taskRepository.UpdateAsync(task);

            return new AddTaskAssigneeCommandResponse {
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
