using Lunatic.Application.Responses;


namespace Lunatic.Application.Features.Tasks.Queries.GetPredictedTaskTime {
	public class GetPredictedTaskTimeCommandResponse : BaseResponse {
		public GetPredictedTaskTimeCommandResponse() : base() { }
		public float Duration { get; set; } = default!;
	}
}
