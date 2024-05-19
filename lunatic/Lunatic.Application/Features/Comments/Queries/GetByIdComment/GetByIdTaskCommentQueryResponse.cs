
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Comments.Payload;


namespace Lunatic.Application.Features.Comments.Queries.GetByIdComment {
    public class GetByIdTaskCommentQueryResponse : BaseResponse {
        public GetByIdTaskCommentQueryResponse() : base() {}

        public CommentDto Comment { get; set; } = default!;
    }
}
