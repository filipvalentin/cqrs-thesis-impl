using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.DeleteComment {
	public class DeleteCommentCommand : IRequest<DeleteCommentCommandResponse> {
		public Guid TaskId { get; set; }
		public Guid CommentId { get; set; }
	}
}
