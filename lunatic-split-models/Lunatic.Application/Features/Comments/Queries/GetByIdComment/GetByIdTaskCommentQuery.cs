
using MediatR;


namespace Lunatic.Application.Features.Comments.Queries.GetByIdComment {
    public class GetByIdTaskCommentQuery : IRequest<GetByIdTaskCommentQueryResponse> {
        public Guid CommentId { get; set; } = default!;
    }
}
