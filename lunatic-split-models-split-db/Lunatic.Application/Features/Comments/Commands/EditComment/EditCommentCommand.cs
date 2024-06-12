using MediatR;

namespace Lunatic.Application.Features.Comments.Commands.EditComment {
	public class EditCommentCommand : IRequest<EditCommentCommandResponse> {
		public Guid CommentId { get; set; } = default!;
		public string Content { get; set; } = default!;
	}
}
