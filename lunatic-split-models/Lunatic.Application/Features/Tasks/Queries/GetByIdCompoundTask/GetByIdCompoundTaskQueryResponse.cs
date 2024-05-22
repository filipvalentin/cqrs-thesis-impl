
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Tasks.Payload;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdCompoundTask
{
    public class GetByIdCompoundTaskQueryResponse : BaseResponse
    {
        public GetByIdCompoundTaskQueryResponse() : base() { }

        public CompoundTaskDto Task { get; set; } = default!;
    }
}
