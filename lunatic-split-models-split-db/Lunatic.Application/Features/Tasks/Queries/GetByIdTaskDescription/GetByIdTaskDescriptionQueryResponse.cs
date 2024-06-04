
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTaskDescription {
	public class GetByIdTaskDescriptionQueryResponse : BaseResponse {
		public GetByIdTaskDescriptionQueryResponse() : base() { }
		public TaskDescriptionDto Task { get; set; } = default!;
	}
}
