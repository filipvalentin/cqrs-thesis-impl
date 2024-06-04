using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Tasks.Commands.DeleteTaskAssignee
{
    internal class DeleteTaskAssigneeCommandValidator : AbstractValidator<DeleteTaskAssigneeCommand> {
        private readonly IUserRepository userRepository;

        private readonly ITaskRepository taskRepository;

        public DeleteTaskAssigneeCommandValidator(IUserRepository userRepository, ITaskRepository taskRepository) {
            this.userRepository = userRepository;
            this.taskRepository = taskRepository;

            RuleFor(request => request.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (userId, cancellationToken) => await this.userRepository.ExistsByIdAsync(userId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => request.TaskId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (teamId, cancellationToken) => await this.taskRepository.ExistsByIdAsync(teamId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => new {request.TaskId, request.UserId})
                .MustAsync(async (req, cancellationToken) => {
                        var task = (await this.taskRepository.FindByIdAsync(req.TaskId)).Value;
                        return task.AssigneeIds.Contains(req.UserId);})
                .WithMessage("Task Assignee must include UserId.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
