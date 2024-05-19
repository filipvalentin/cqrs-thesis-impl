
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Comments.Payload;


namespace Lunatic.Application.Features.Tasks.Commands.CreateComment
{
    public class CreateCommentCommandResponse : BaseResponse
    {
        public CreateCommentCommandResponse() : base() { }

        public CommentDto Comment { get; set; } = default!;
    }
}
