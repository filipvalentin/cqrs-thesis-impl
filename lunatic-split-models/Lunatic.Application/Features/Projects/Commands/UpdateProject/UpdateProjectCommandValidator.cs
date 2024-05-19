
using Lunatic.Application.Persistence;
using FluentValidation;


namespace Lunatic.Application.Features.Projects.Commands.UpdateProject
{
    internal class UpdateTeamProjectCommandValidator : AbstractValidator<UpdateTeamProjectCommand>
    {
        private readonly IProjectRepository projectRepository;

        public UpdateTeamProjectCommandValidator(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;

            RuleFor(request => request.ProjectId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (projectId, cancellationToken) => await this.projectRepository.ExistsByIdAsync(projectId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => request.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(request => request.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MaximumLength(5000).WithMessage("{PropertyName} must not exceed 5000 characters.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
