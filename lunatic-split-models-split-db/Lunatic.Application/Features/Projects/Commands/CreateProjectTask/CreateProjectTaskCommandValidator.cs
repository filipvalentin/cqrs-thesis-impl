
using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Projects.Commands.CreateProjectTask
{
    internal class CreateProjectTaskCommandValidator : AbstractValidator<CreateProjectTaskCommand> {
		private readonly IUserRepository userRepository;

		private readonly IProjectRepository projectRepository;

		public CreateProjectTaskCommandValidator(IUserRepository userRepository, IProjectRepository projectRepository) {
			this.userRepository = userRepository;
			this.projectRepository = projectRepository;

			RuleFor(request => request.UserId)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MustAsync(async (userId, cancellationToken) => await this.userRepository.ExistsByIdAsync(userId))
				.WithMessage("{PropertyName} must exists.");

			RuleFor(request => request.ProjectId)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MustAsync(async (projectId, cancellationToken) => await this.projectRepository.ExistsByIdAsync(projectId))
				.WithMessage("{PropertyName} must exists.");

			RuleFor(request => request.Section)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

			RuleFor(request => new { request.ProjectId, request.Section })
				.MustAsync(async (req, cancellationToken) => {
					var project = (await this.projectRepository.FindByIdAsync(req.ProjectId)).Value;
					return project.TaskSectionCards.Contains(req.Section);
				})
				.WithMessage("Project must include Task Section.");

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
