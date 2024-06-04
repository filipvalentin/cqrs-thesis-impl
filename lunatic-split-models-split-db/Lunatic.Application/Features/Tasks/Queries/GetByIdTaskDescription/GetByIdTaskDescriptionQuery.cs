using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTaskDescription {
	public class GetByIdTaskDescriptionQuery : IRequest<GetByIdTaskDescriptionQueryResponse> {
		public Guid TaskId { get; set; } = default!;
	}
}
