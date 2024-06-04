
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTask
{
    public class GetByIdTaskQueryResponse : BaseResponse
    {
        public GetByIdTaskQueryResponse() : base() { }

        public TaskDto Task { get; set; } = default!;
    }
}
