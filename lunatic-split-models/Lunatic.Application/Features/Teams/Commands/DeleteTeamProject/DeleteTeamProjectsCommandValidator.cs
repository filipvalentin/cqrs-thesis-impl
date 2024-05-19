
using Lunatic.Application.Persistence;
using FluentValidation;


namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamProject
{
    internal class DeleteTeamProjectCommandValidator : AbstractValidator<DeleteTeamProjectCommand>
    {
        private readonly ITeamRepository teamRepository;

        private readonly IProjectRepository projectRepository;

        public DeleteTeamProjectCommandValidator(ITeamRepository teamRepository, IProjectRepository projectRepository)
        {
            this.teamRepository = teamRepository;
            this.projectRepository = projectRepository;

            RuleFor(request => request.TeamId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (teamId, cancellationToken) => await this.teamRepository.ExistsByIdAsync(teamId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => request.ProjectId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (projectId, cancellationToken) => await this.projectRepository.ExistsByIdAsync(projectId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => new { request.TeamId, request.ProjectId })
                .MustAsync(async (req, cancellationToken) =>
                {
                    var team = (await this.teamRepository.FindByIdAsync(req.TeamId)).Value;
                    return team.ProjectIds.Contains(req.ProjectId);
                })
                .WithMessage("Team must include ProjectId.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
