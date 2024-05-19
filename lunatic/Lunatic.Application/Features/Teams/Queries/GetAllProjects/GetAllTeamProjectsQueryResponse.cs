
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Projects.Payload;


namespace Lunatic.Application.Features.Teams.Queries.GetAllProjects {
    public class GetAllTeamProjectsQueryResponse : BaseResponse {
        public GetAllTeamProjectsQueryResponse() : base() {}

        public List<ProjectDto> Projects { get; set; } = default!;
    }
}
