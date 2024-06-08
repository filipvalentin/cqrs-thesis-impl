using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;

namespace Lunatic.Application.Features.Projects.Commands.RenameTaskSection {
	internal class RenameTaskSectionCardCommandValidator : AbstractValidator<RenameTaskSectionCardCommand> {
		private readonly IProjectRepository projectRepository;

		public RenameTaskSectionCardCommandValidator(IProjectRepository projectRepository) {
			this.projectRepository = projectRepository;

			RuleFor(request => request.ProjectId)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MustAsync(async (projectId, cancellationToken) => await this.projectRepository.ExistsByIdAsync(projectId))
				.WithMessage("{PropertyName} must exists.");

			RuleFor(request => request.Section)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MaximumLength(25).WithMessage("{PropertyName} must not exceed 25 characters.");

			RuleFor(request => request.NewSection)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MaximumLength(25).WithMessage("{PropertyName} must not exceed 25 characters.");

			RuleFor(request => new { request.ProjectId, request.Section })
				.MustAsync(async (req, cancellationToken) => {
					var project = (await this.projectRepository.FindByIdAsync(req.ProjectId)).Value;
					return project.TaskSectionCards.Contains(req.Section);
				})
				.WithMessage("Project must include Section.");

			RuleFor(request => new { request.ProjectId, request.NewSection })
				.MustAsync(async (req, cancellationToken) => {
					var project = (await this.projectRepository.FindByIdAsync(req.ProjectId)).Value;
					return !project.TaskSectionCards.Contains(req.NewSection);
				})
				.WithMessage("Project must not include NewSection.");

			ClassLevelCascadeMode = CascadeMode.Stop;
		}
	}
}
