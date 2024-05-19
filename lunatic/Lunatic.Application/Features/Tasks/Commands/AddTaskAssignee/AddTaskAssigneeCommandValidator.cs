
using Lunatic.Application.Persistence;
using FluentValidation;


namespace Lunatic.Application.Features.Tasks.Commands.AddTaskAssignee {
    internal class AddTaskAssigneeCommandValidator : AbstractValidator<AddTaskAssigneeCommand> {
        private readonly IUserRepository userRepository;

        private readonly ITaskRepository taskRepository;

        public AddTaskAssigneeCommandValidator(IUserRepository userRepository, ITaskRepository taskRepository) {
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
                        return !task.AssigneeIds.Contains(req.UserId);})
                .WithMessage("UserId exists already in Task Assignee List.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
