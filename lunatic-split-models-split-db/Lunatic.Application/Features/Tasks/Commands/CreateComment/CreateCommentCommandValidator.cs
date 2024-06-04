using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Tasks.Commands.CreateComment
{
    internal class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        private readonly IUserRepository userRepository;

        private readonly ITaskRepository taskRepository;

        public CreateCommentCommandValidator(IUserRepository userRepository, ITaskRepository taskRepository)
        {
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
                .MustAsync(async (taskId, cancellationToken) => await this.taskRepository.ExistsByIdAsync(taskId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => request.Content)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MaximumLength(3000).WithMessage("{PropertyName} must not exceed 3000 characters.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
