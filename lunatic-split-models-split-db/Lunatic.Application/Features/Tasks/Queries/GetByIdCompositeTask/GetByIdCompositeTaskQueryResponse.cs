
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdCompositeTask {
	public class GetByIdCompositeTaskQueryResponse : BaseResponse {
		public GetByIdCompositeTaskQueryResponse() : base() { }

		public CompositeTaskDto Task { get; set; } = default!;
	}
}
