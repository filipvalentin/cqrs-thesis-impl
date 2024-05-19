using Lunatic.Application.Persistence;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamProject {
	public class DeleteTeamProjectCommandHandler : IRequestHandler<DeleteTeamProjectCommand, DeleteTeamProjectCommandResponse> {
		private readonly ITeamRepository teamRepository;

		private readonly IProjectRepository projectRepository;

		public DeleteTeamProjectCommandHandler(ITeamRepository teamRepository, IProjectRepository projectRepository) {
			this.teamRepository = teamRepository;
			this.projectRepository = projectRepository;
		}

		public async Task<DeleteTeamProjectCommandResponse> Handle(DeleteTeamProjectCommand request, CancellationToken cancellationToken) {
			var validator = new DeleteTeamProjectCommandValidator(teamRepository, projectRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new DeleteTeamProjectCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var team = (await teamRepository.FindByIdAsync(request.TeamId)).Value;
			team.RemoveProject(request.ProjectId);
			await teamRepository.UpdateAsync(team);

			var result = await projectRepository.DeleteAsync(request.ProjectId);

			if (!result.IsSuccess) {
				return new DeleteTeamProjectCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};

			}
			return new DeleteTeamProjectCommandResponse {
				Success = true
			};
		}
	}
}
