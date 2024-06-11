using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.ReadSide;
using MediatR;

namespace Lunatic.Application.Features.Projects.Queries.GetByIdProject {
	public class GetByIdProjectQueryHandler : IRequestHandler<GetByIdProjectQuery, GetByIdProjectQueryResponse> {
		private readonly IProjectReadSideRepository projectRepository;
		private readonly IMapper mapper;
		public GetByIdProjectQueryHandler(IProjectReadSideRepository projectRepository, IMapper mapper) {
			this.projectRepository = projectRepository;
			this.mapper = mapper;
		}

		public async Task<GetByIdProjectQueryResponse> Handle(GetByIdProjectQuery request, CancellationToken cancellationToken) {
			var project = await projectRepository.FindByIdAsync(request.ProjectId);
			if (!project.IsSuccess) {
				return new GetByIdProjectQueryResponse {
					Success = false,
					Message = project.Error
				};
			}

			return new GetByIdProjectQueryResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(project.Value)
			};
		}
	}
}
