
using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTask
{
    internal class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand> {
		private readonly ITaskRepository taskRepository;

		public UpdateTaskCommandValidator(ITaskRepository taskRepository) {
			this.taskRepository = taskRepository;

			RuleFor(request => request.TaskId)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MustAsync(async (taskId, cancellationToken) => await this.taskRepository.ExistsByIdAsync(taskId))
				.WithMessage("{PropertyName} must exists.");

			RuleFor(request => request.Title)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

			RuleFor(request => request.Description)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MaximumLength(5000).WithMessage("{PropertyName} must not exceed 5000 characters.");

			RuleFor(request => request.Priority)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.IsInEnum().WithMessage("{PropertyName} is not a valid priority.");

			RuleFor(request => request.PlannedStartDate)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.");

			RuleFor(request => request.PlannedEndDate)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.");

			RuleFor(request => new { request.PlannedStartDate, request.PlannedEndDate })
				.Must((req, cancellationtoken) => {
					return req.PlannedStartDate <= req.PlannedEndDate;
				})
				.WithMessage("PlannedStartDate must be less than or equal to PlannedEndDate");

			ClassLevelCascadeMode = CascadeMode.Stop;
		}
	}
}
