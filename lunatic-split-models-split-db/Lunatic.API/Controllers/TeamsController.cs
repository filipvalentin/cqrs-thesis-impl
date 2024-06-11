using Lunatic.Application.Features.Teams.Commands.AddTeamMember;
using Lunatic.Application.Features.Teams.Commands.AddTeamProject;
using Lunatic.Application.Features.Teams.Commands.CreateTeam;
using Lunatic.Application.Features.Teams.Commands.DeleteTeam;
using Lunatic.Application.Features.Teams.Commands.DeleteTeamMember.cs;
using Lunatic.Application.Features.Teams.Commands.DeleteTeamProject;
using Lunatic.Application.Features.Teams.Commands.UpdateTeam;
using Lunatic.Application.Features.Teams.Queries.GetAllMembers;
using Lunatic.Application.Features.Teams.Queries.GetById;
using Microsoft.AspNetCore.Mvc;

namespace Lunatic.API.Controllers {
	public class TeamsController : BaseApiController {
		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType<CreateTeamCommandResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<CreateTeamCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create(CreateTeamCommand command) {
			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		//[HttpGet]
		//[Produces("application/json")]
		//[ProducesResponseType<GetAllTeamsQueryResponse>(StatusCodes.Status200OK)]
		//public async Task<IActionResult> GetAll() {
		//	var result = await Mediator.Send(new GetAllTeamsQuery());
		//	return Ok(result);
		//}

		[HttpPut("{teamId}")]
		[Produces("application/json")]
		[ProducesResponseType<UpdateTeamCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<UpdateTeamCommandResponse>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<UpdateTeamCommandResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(Guid teamId, UpdateTeamCommand command) {
			if (teamId != command.TeamId) {
				return BadRequest(new UpdateTeamCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The Id Path and Id Body must be equal." }
				});
			}

			var existsResult = await Mediator.Send(new GetByIdTeamQuery(teamId));
			if (!existsResult.Success) {
				return NotFound(existsResult);
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpDelete("{teamId}")]
		[Produces("application/json")]
		[ProducesResponseType<DisbandTeamCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<DisbandTeamCommandResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(Guid teamId) {
			var deleteTeamCommand = new DisbandTeamCommand() { TeamId = teamId };
			var result = await Mediator.Send(deleteTeamCommand);
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		[HttpGet("{teamId}")]
		[Produces("application/json")]
		[ProducesResponseType<GetByIdTeamQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetByIdTeamQueryResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(Guid teamId) {
			var result = await Mediator.Send(new GetByIdTeamQuery(teamId));
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		[HttpGet("{teamId}/members")]
		[Produces("application/json")]
		[ProducesResponseType<GetAllTeamMembersQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetAllTeamMembersQueryResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetAllMembers(Guid teamId) {
			var result = await Mediator.Send(new GetAllTeamMembersQuery(teamId));
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		[HttpPost("{teamId}/members")]
		[Produces("application/json")]
		[ProducesResponseType<AddTeamMemberCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<AddTeamMemberCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddMember(Guid teamId, AddTeamMemberCommand command) {
			if (teamId != command.TeamId) {
				return BadRequest(new AddTeamMemberCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The team Id Path and team Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpPost("{teamId}/projects")]
		[Produces("application/json")]
		[ProducesResponseType<CreateProjectCommandResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<CreateProjectCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateProject(Guid teamId, CreateProjectCommand command) {
			if (teamId != command.TeamId) {
				return BadRequest(new CreateProjectCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The team Id Path and team Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}



		[HttpDelete("{teamId}/projects/{projectId}")]
		[Produces("application/json")]
		[ProducesResponseType<DeleteTeamProjectCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<DeleteTeamProjectCommandResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteProject(Guid teamId, Guid projectId) {
			var deleteTeamProjectCommand = new DeleteTeamProjectCommand() {
				TeamId = teamId,
				ProjectId = projectId
			};
			var result = await Mediator.Send(deleteTeamProjectCommand);
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		[HttpDelete("{teamId}/members/{userId}")]
		[Produces("application/json")]
		[ProducesResponseType<DeleteTeamMemberCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<DeleteTeamMemberCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteMember(Guid teamId, Guid userId) {
			var deleteTeamMemberCommand = new DeleteTeamMemberCommand() {
				TeamId = teamId,
				UserId = userId
			};
			var result = await Mediator.Send(deleteTeamMemberCommand);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}

