
using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Teams.Commands.AddTeamProject
{
    internal class CreateTeamProjectCommandValidator : AbstractValidator<CreateTeamProjectCommand>
    {
        private readonly IUserRepository userRepository;

        private readonly ITeamRepository teamRepository;

        public CreateTeamProjectCommandValidator(IUserRepository userRepository, ITeamRepository teamRepository)
        {
            this.userRepository = userRepository;
            this.teamRepository = teamRepository;

            RuleFor(request => request.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (userId, cancellationToken) => await this.userRepository.ExistsByIdAsync(userId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => request.TeamId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (teamId, cancellationToken) => await this.teamRepository.ExistsByIdAsync(teamId))
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
