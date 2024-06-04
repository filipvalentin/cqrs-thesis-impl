using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Tasks.Commands.DeleteTaskTag
{
    internal class DeleteTaskTagCommandValidator : AbstractValidator<DeleteTaskTagCommand> {
        private readonly ITaskRepository taskRepository;

        public DeleteTaskTagCommandValidator(ITaskRepository taskRepository) {
            this.taskRepository = taskRepository;

            RuleFor(request => request.TaskId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (teamId, cancellationToken) => await this.taskRepository.ExistsByIdAsync(teamId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => request.Tag)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(request => new {request.TaskId, request.Tag})
                .MustAsync(async (req, cancellationToken) => {
                        var task = (await this.taskRepository.FindByIdAsync(req.TaskId)).Value;
                        return task.Tags.Contains(req.Tag);})
                .WithMessage("Task Tags must include Tag.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
