using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Teams.Commands.UpdateTeam
{
    internal class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand> {
        private readonly ITeamRepository teamRepository;

        public UpdateTeamCommandValidator(ITeamRepository teamRepository) {
            this.teamRepository = teamRepository;

            RuleFor(request => request.TeamId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (teamId, cancellationToken) => await this.teamRepository.ExistsByIdAsync(teamId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MaximumLength(30).WithMessage("{PropertyName} must not exceed 30 characters.");

            RuleFor(request => request.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MaximumLength(5000).WithMessage("{PropertyName} must not exceed 100 characters.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
