using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdFlatTask {
	public class GetByIdFlatTaskQuery : IRequest<GetByIdFlatTaskQueryResponse> {
		public Guid TaskId { get; set; } = default!;
	}
}
