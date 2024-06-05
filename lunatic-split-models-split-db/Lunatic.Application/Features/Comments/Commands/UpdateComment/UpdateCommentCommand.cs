using MediatR;

namespace Lunatic.Application.Features.Comments.Commands.UpdateComment {
	public class UpdateCommentCommand : IRequest<UpdateTaskCommentCommandResponse> {
		public Guid CommentId { get; set; } = default!;
		public string Content { get; set; } = default!;
	}
}
