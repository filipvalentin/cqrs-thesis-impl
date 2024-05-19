using MediatR;


namespace Lunatic.Application.Features.Projects.Queries.GetByIdProject {
	public class GetByIdProjectQuery : IRequest<GetByIdProjectQueryResponse> {
		public Guid ProjectId { get; set; } = default!;
	}
}
