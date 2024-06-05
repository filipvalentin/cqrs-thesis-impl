using Lunatic.Application.Responses;
using Lunatic.Application.Features.Comments.Payload;

namespace Lunatic.Application.Features.Comments.Queries.GetByIdComment {
    public class GetByIdCommentQueryResponse : BaseResponse {
        public GetByIdCommentQueryResponse() : base() {}
        public CommentDto Comment { get; set; } = default!;
    }
}
