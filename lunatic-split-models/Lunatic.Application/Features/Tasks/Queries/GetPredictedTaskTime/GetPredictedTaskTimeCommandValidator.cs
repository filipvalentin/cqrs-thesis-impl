
using Lunatic.Application.Persistence;
using FluentValidation;


namespace Lunatic.Application.Features.Tasks.Queries.GetPredictedTaskTime
{
    internal class GetPredictedTaskTimeCommandValidator : AbstractValidator<GetPredictedTaskTimeCommand>
    {
        private readonly ITaskRepository taskRepository;

        public GetPredictedTaskTimeCommandValidator(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;

            RuleFor(request => request.TaskId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (taskId, cancellationToken) => await this.taskRepository.ExistsByIdAsync(taskId))
                .WithMessage("{PropertyName} must exists.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
