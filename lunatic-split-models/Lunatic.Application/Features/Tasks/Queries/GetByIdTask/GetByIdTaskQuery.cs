using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTask {
	public class GetByIdTaskQuery : IRequest<GetByIdTaskQueryResponse> {
		public Guid TaskId { get; set; } = default!;
	}
}
