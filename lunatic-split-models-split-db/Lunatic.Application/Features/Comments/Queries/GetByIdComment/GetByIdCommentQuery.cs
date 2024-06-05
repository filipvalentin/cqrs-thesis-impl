using MediatR;

namespace Lunatic.Application.Features.Comments.Queries.GetByIdComment {
	public class GetByIdCommentQuery : IRequest<GetByIdCommentQueryResponse> {
		public Guid CommentId { get; set; } = default!;
	}
}
