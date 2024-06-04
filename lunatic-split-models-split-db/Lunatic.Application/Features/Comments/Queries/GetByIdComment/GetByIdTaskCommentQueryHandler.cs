
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Comments.Queries.GetByIdComment
{
    public class GetByIdTaskCommentQueryHandler : IRequestHandler<GetByIdTaskCommentQuery, GetByIdTaskCommentQueryResponse> {
		private readonly ICommentRepository commentRepository;

		public GetByIdTaskCommentQueryHandler(ICommentRepository commentRepository) {
			this.commentRepository = commentRepository;
		}

		public async Task<GetByIdTaskCommentQueryResponse> Handle(GetByIdTaskCommentQuery request, CancellationToken cancellationToken) {
			var validator = new GetByIdTaskCommentQueryValidator(commentRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new GetByIdTaskCommentQueryResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

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
