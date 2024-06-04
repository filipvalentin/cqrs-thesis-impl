using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Projects.Queries.GetAllTaskSections
{
    public class GetAllProjectTaskSectionsQueryHandler : IRequestHandler<GetAllProjectTaskSectionsQuery, GetAllProjectTaskSectionsQueryResponse> {
        private readonly IProjectRepository projectRepository;

        public GetAllProjectTaskSectionsQueryHandler(IProjectRepository projectRepository) {
            this.projectRepository = projectRepository;
        }

        public async Task<GetAllProjectTaskSectionsQueryResponse> Handle(GetAllProjectTaskSectionsQuery request, CancellationToken cancellationToken) {
            var projectResult = await projectRepository.FindByIdAsync(request.ProjectId);
            if(!projectResult.IsSuccess) {
                return new GetAllProjectTaskSectionsQueryResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Project not found" }
                };
            }

            GetAllProjectTaskSectionsQueryResponse response = new GetAllProjectTaskSectionsQueryResponse();
            var taskSections = projectResult.Value.TaskSectionCards;

            response.Sections = taskSections;
            return response;
        }
    }
}
