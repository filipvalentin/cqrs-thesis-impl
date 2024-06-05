
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.ReadSide;
using MediatR;


namespace Lunatic.Application.Features.Comments.Queries.GetByIdComment {
	public class GetByIdTaskCommentQueryHandler : IRequestHandler<GetByIdTaskCommentQuery, GetByIdTaskCommentQueryResponse> {
		private readonly ICommentReadSideRepository commentRepository;

		public GetByIdTaskCommentQueryHandler(ICommentReadSideRepository commentRepository) {
			this.commentRepository = commentRepository;
		}

		public async Task<GetByIdTaskCommentQueryResponse> Handle(GetByIdTaskCommentQuery request, CancellationToken cancellationToken) {
	
			var comment = (await commentRepository.FindByIdAsync(request.CommentId)).Value;

			return new GetByIdTaskCommentQueryResponse {
				Success = true,
				Comment = new CommentDto {
					CommentId = comment.CommentId,
					TaskId = comment.TaskId,
					AuthorId = comment.CreatedByUserId,

					Content = comment.Content,

					//EmoteIds = comment.EmoteIds,

					CreatedDate = comment.CreatedDate,
					LastModifiedDate = comment.LastModifiedDate
				}
			};
		}
	}
}
