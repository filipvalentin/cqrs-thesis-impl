using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdCompositeTask {
	public class GetByIdCompositeTaskQuery : IRequest<GetByIdCompositeTaskQueryResponse> {
		public Guid TaskId { get; set; } = default!;
	}
}
