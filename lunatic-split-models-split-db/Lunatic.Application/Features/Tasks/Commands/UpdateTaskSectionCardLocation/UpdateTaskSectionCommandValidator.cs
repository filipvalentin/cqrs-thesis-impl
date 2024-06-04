
using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskSectionCardLocation
{
    internal class UpdateTaskSectionCommandValidator : AbstractValidator<UpdateTaskSectionLocationCommand> {
		private readonly ITaskRepository taskRepository;

		private readonly IProjectRepository projectRepository;

		public UpdateTaskSectionCommandValidator(ITaskRepository taskRepository, IProjectRepository projectRepository) {
			this.taskRepository = taskRepository;
			this.projectRepository = projectRepository;

			RuleFor(request => request.TaskId)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MustAsync(async (taskId, cancellationToken) => await this.taskRepository.ExistsByIdAsync(taskId))
				.WithMessage("{PropertyName} must exists.");

			RuleFor(request => request.Section)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MaximumLength(25).WithMessage("{PropertyName} must not exceed 25 characters.");

			RuleFor(request => new { request.TaskId, request.Section })
				.MustAsync(async (req, cancellationToken) => {
					var task = (await this.taskRepository.FindByIdAsync(req.TaskId)).Value;
					var project = (await this.projectRepository.FindByIdAsync(task.ProjectId)).Value;
					return project.TaskSectionCards.Contains(req.Section);
				})
				.WithMessage("Project must include Section.");

			ClassLevelCascadeMode = CascadeMode.Stop;
		}
	}
}
