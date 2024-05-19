
using Lunatic.Application.Persistence;
using FluentValidation;


namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamMember.cs {
    internal class DeleteTeamMemberCommandValidator : AbstractValidator<DeleteTeamMemberCommand> {
        private readonly IUserRepository userRepository;

        private readonly ITeamRepository teamRepository;

        public DeleteTeamMemberCommandValidator(IUserRepository userRepository, ITeamRepository teamRepository) {
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

            RuleFor(request => new {request.TeamId, request.UserId})
                .MustAsync(async (req, cancellationToken) => {
                        var team = (await this.teamRepository.FindByIdAsync(req.TeamId)).Value;
                        return team.MemberIds.Contains(req.UserId);})
                .WithMessage("Team must include UserId.");

			ClassLevelCascadeMode = CascadeMode.Stop;
		}
    }
}
