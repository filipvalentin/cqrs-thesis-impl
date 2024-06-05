
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdCompositeTask {
	public class GetByIdFlatTaskQueryResponse : BaseResponse {
		public GetByIdFlatTaskQueryResponse() : base() { }

		public FlatTaskDto Task { get; set; } = default!;
	}
}
