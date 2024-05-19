
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTask
{
    public class GetByIdProjectTaskQueryResponse : BaseResponse
    {
        public GetByIdProjectTaskQueryResponse() : base() { }

        public TaskDto Task { get; set; } = default!;
    }
}
