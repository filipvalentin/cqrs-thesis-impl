
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Projects.Queries.GetByIdProject
{
    public class GetByIdProjectQueryHandler : IRequestHandler<GetByIdProjectQuery, GetByIdProjectQueryResponse> {
		private readonly IProjectRepository projectRepository;

		public GetByIdProjectQueryHandler(IProjectRepository projectRepository) {
			this.projectRepository = projectRepository;
		}

		public async Task<GetByIdProjectQueryResponse> Handle(GetByIdProjectQuery request, CancellationToken cancellationToken) {
			var validator = new GetByIdProjectQueryValidator(projectRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new GetByIdProjectQueryResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var project = (await projectRepository.FindByIdAsync(request.ProjectId)).Value;

			return new GetByIdProjectQueryResponse {
				Success = true,
				Project = new ProjectDto {
					CreatedByUserId = project.CreatedByUserId,
					ProjectId = project.Id,
					TeamId = project.TeamId,

					Title = project.Title,
					Description = project.Description,

					TaskSections = project.TaskSectionCards,
					TaskIds = project.TaskIds,
				}
			};
		}
	}
}
