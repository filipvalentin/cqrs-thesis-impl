
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Projects.Payload;


namespace Lunatic.Application.Features.Projects.Queries.GetByIdProject {
	public class GetByIdProjectQueryResponse : BaseResponse {
		public GetByIdProjectQueryResponse() : base() { }

		public ProjectDto Project { get; set; } = default!;
	}
}
