
using FluentValidation;
using Lunatic.Application.Persistence;


namespace Lunatic.Application.Features.Projects.Commands.CreateTaskSectionCard
{
    internal class CreateTaskSectionCommandValidator : AbstractValidator<CreateTaskSectionCommand>
    {
        private readonly IProjectRepository projectRepository;

        public CreateTaskSectionCommandValidator(IProjectRepository projectRepository)
        {
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

            RuleFor(request => new { request.ProjectId, request.Section })
                .MustAsync(async (req, cancellationToken) =>
                {
                    var project = (await this.projectRepository.FindByIdAsync(req.ProjectId)).Value;
                    return !project.TaskSectionCards.Contains(req.Section);
                })
                .WithMessage("Project must not include Task Section.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
