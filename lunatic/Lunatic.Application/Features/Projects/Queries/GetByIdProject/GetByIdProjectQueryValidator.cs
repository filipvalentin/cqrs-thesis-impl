
using Lunatic.Application.Persistence;
using FluentValidation;


namespace Lunatic.Application.Features.Projects.Queries.GetByIdProject {
	internal class GetByIdProjectQueryValidator : AbstractValidator<GetByIdProjectQuery> {
		private readonly IProjectRepository projectRepository;

		public GetByIdProjectQueryValidator(IProjectRepository projectRepository) {
			this.projectRepository = projectRepository;

			RuleFor(request => request.ProjectId)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MustAsync(async (projectId, cancellationToken) => await this.projectRepository.ExistsByIdAsync(projectId))
				.WithMessage("{PropertyName} must exists.");

			ClassLevelCascadeMode = CascadeMode.Stop;
		}
	}
}

