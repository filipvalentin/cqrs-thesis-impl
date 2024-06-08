using Lunatic.Application.Features.Users.Commands.CreateUser;
using Lunatic.Application.Features.Users.Commands.DeleteUser;
using Lunatic.Application.Features.Users.Commands.UpdateUser;
using Lunatic.Application.Features.Users.Queries.GetAllUserTeams;
using Lunatic.Application.Features.Users.Queries.GetById;
using Lunatic.Application.Features.Users.Queries.GetUsernameMatches;
using Microsoft.AspNetCore.Mvc;

namespace Lunatic.API.Controllers {
	public class UsersController : BaseApiController {
		//[HttpPost]
		//[Produces("application/json")]
		//[ProducesResponseType<CreateUserCommandResponse>(StatusCodes.Status201Created)]
		//[ProducesResponseType<CreateUserCommandResponse>(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> Create(CreateUserCommand command) {
		//	var result = await Mediator.Send(command);
		//	if (!result.Success) {
		//		return BadRequest(result);
		//	}
		//	return Ok(result);
		//}

		[HttpPut("{userId}")]
		[Produces("application/json")]
		[ProducesResponseType<UpdateUserCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<UpdateUserCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateUser(Guid userId, UpdateUserCommand command) {
			if (userId != command.UserId) {
				return BadRequest(new UpdateUserCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The user Id Path and user Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpDelete("{userId}")]
		[Produces("application/json")]
		[ProducesResponseType<DeleteUserCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<DeleteUserCommandResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteUser(Guid userId) {
			var deleteUserCommand = new DeleteUserCommand() { UserId = userId };
			var result = await Mediator.Send(deleteUserCommand);
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		//[HttpGet]
		//[Produces("application/json")]
		//[ProducesResponseType<GetUsernameMatchesQueryResponse>(StatusCodes.Status200OK)]
		//[ProducesResponseType<GetUsernameMatchesQueryResponse>(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> GetAll([FromQuery] string username = default!) {
		//    var getAllUsersQuery = new GetUsernameMatchesQuery {
		//        Username = username
		//    };
		//    var result = await Mediator.Send(getAllUsersQuery);
		//    return Ok(result);
		//}
		[HttpGet("usernames/{startingWith}")]
		[Produces("application/json")]
		[ProducesResponseType<GetUsernameMatchesQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetUsernameMatchesQueryResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetUsernameMatches(string startingWith, [FromQuery] int take = 10) {
			var getAllUsersQuery = new GetUsernameMatchesQuery {
				StartingWith = startingWith,
				Take = take
			};
			var result = await Mediator.Send(getAllUsersQuery);
			return Ok(result.Matches);
		}

		[HttpGet("{userId}")]
		[Produces("application/json")]
		[ProducesResponseType<GetByIdUserQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetByIdUserQueryResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByIdUser(Guid userId) {
			var result = await Mediator.Send(new GetByIdUserQuery(userId));
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		[HttpGet("{userId}/teams")]
		[Produces("application/json")]
		[ProducesResponseType<GetAllUserTeamsQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetAllUserTeamsQueryResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTeams(Guid userId) {
			var result = await Mediator.Send(new GetAllUserTeamsQuery(userId));
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}
	}
}

