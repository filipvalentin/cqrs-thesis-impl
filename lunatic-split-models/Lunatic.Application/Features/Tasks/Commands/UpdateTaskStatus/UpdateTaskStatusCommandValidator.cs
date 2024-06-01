
using Lunatic.Application.Persistence;
using FluentValidation;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    internal class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        private readonly ITaskRepository taskRepository;

        public UpdateTaskStatusCommandValidator(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;

            RuleFor(request => request.TaskId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (taskId, cancellationToken) => await this.taskRepository.ExistsByIdAsync(taskId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => request.Status)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .IsInEnum().WithMessage("{PropertyName} is not a valid status.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
