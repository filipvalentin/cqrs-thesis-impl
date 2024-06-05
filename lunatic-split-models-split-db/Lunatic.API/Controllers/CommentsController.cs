
using Lunatic.Application.Features.Comments.Commands.UpdateComment;
using Lunatic.Application.Features.Comments.Queries.GetByIdComment;
using Microsoft.AspNetCore.Mvc;

namespace Lunatic.API.Controllers {
	public class CommentsController : BaseApiController {

		[HttpGet("{commentId}")]
		[Produces("application/json")]
		[ProducesResponseType<GetByIdCommentQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetByIdCommentQueryResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByIdComment(Guid commentId) {
			var result = await Mediator.Send(new GetByIdCommentQuery {
				CommentId = commentId
			});
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		[HttpPut("{commentId}")]
		[Produces("application/json")]
		[ProducesResponseType<UpdateTaskCommentCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<UpdateTaskCommentCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateComment(Guid commentId, UpdateCommentCommand command) {
			if (commentId != command.CommentId) {
				return BadRequest(new UpdateTaskCommentCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The comment Id Path and comment Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}

