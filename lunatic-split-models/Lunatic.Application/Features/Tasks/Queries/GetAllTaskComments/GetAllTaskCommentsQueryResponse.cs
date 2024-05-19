
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Comments.Payload;


namespace Lunatic.Application.Features.Tasks.Queries.GetAllTaskComments
{
    public class GetAllTaskCommentsQueryResponse : BaseResponse
    {
        public GetAllTaskCommentsQueryResponse() : base() { }

        public List<CommentDto> Comments { get; set; } = default!;
    }
}
