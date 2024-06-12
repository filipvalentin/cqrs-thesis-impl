using Lunatic.Application.Responses;
using Lunatic.Application.Features.Comments.Payload;

namespace Lunatic.Application.Features.Comments.Commands.EditComment {
	public class EditCommentCommandResponse : BaseResponse {
		public EditCommentCommandResponse() : base() { }
		public CommentDto Comment { get; set; } = default!;
	}
}
