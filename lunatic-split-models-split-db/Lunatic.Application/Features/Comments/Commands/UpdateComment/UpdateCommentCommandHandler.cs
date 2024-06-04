
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, UpdateTaskCommentCommandResponse> {
		private readonly ICommentRepository commentRepository;

		public UpdateCommentCommandHandler(ICommentRepository commentRepository) {
			this.commentRepository = commentRepository;
		}

		public async Task<UpdateTaskCommentCommandResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken) {
			var validator = new UpdateCommentCommandValidator(commentRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new UpdateTaskCommentCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var commentResult = await commentRepository.FindByIdAsync(request.CommentId);

			commentResult.Value.Update(request.Content);

			var dbCommentResult = await commentRepository.UpdateAsync(commentResult.Value);

			return new UpdateTaskCommentCommandResponse {
				Success = true,
				Comment = new CommentDto {
					CommentId = dbCommentResult.Value.CommentId,
					TaskId = dbCommentResult.Value.TaskId,
					AuthorId = dbCommentResult.Value.CreatedByUserId,

					Content = dbCommentResult.Value.Content,

					CreatedDate = dbCommentResult.Value.CreatedDate,
					LastModifiedDate = dbCommentResult.Value.LastModifiedDate
				}
			};
		}
	}
}
