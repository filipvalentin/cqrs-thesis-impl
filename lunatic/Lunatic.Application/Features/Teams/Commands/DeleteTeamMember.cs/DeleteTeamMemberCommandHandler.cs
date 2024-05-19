
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Teams.Payload;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamMember.cs {
    public class DeleteTeamMemberCommandHandler : IRequestHandler<DeleteTeamMemberCommand, DeleteTeamMemberCommandResponse> {
        private readonly ITeamRepository teamRepository;

        private readonly IUserRepository userRepository;

        public DeleteTeamMemberCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository) {
            this.teamRepository = teamRepository;
            this.userRepository = userRepository;
        }

        public async Task<DeleteTeamMemberCommandResponse> Handle(DeleteTeamMemberCommand request, CancellationToken cancellationToken) {
            var validator = new DeleteTeamMemberCommandValidator(userRepository, teamRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if(!validatorResult.IsValid) {
                return new DeleteTeamMemberCommandResponse {
                    Success = false,
                    ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var team = (await teamRepository.FindByIdAsync(request.TeamId)).Value;
            team.RemoveMember(request.UserId);
            var dbTeamResult = await teamRepository.UpdateAsync(team);

            return new DeleteTeamMemberCommandResponse {
                Success = true,
                Team = new TeamDto {
                    TeamId = dbTeamResult.Value.Id,

                    Name = dbTeamResult.Value.Name,
                    Description = dbTeamResult.Value.Description,

                    MemberIds = dbTeamResult.Value.MemberIds,
                    ProjectIds = dbTeamResult.Value.ProjectIds,
                }
            };
        }
    }
}
