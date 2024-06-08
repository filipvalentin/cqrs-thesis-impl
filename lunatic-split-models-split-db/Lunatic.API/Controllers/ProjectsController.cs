using Lunatic.Application.Features.Projects.Commands.CreateTask;
using Lunatic.Application.Features.Projects.Commands.CreateTaskSectionCard;
using Lunatic.Application.Features.Projects.Commands.DeleteTask;
using Lunatic.Application.Features.Projects.Commands.DeleteTaskSectionCard;
using Lunatic.Application.Features.Projects.Commands.RenameTaskSection;
using Lunatic.Application.Features.Projects.Commands.UpdateProject;
using Lunatic.Application.Features.Projects.Queries.GetByIdProject;
using Microsoft.AspNetCore.Mvc;

namespace Lunatic.API.Controllers {
	public class ProjectsController : BaseApiController {


		[HttpPost("{projectId}/tasks")]
		[Produces("application/json")]
		[ProducesResponseType<CreateTaskCommandResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<CreateTaskCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateTask(Guid projectId, CreateTaskCommand command) {
			if (projectId != command.ProjectId) {
				return BadRequest(new CreateTaskCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The project Id Path and project Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		//[HttpGet("{projectId}/tasks")]
		//[Produces("application/json")]
		//[ProducesResponseType<GetAllProjectTasksQueryResponse>(StatusCodes.Status200OK)]
		//[ProducesResponseType<GetAllProjectTasksQueryResponse>(StatusCodes.Status404NotFound)]
		//public async Task<IActionResult> GetAllTasks(Guid projectId) {
		//	var result = await Mediator.Send(new GetAllProjectTasksQuery(projectId));
		//	if (!result.Success) {
		//		return NotFound(result);
		//	}
		//	return Ok(result);
		//}

		[HttpPut("{projectId}")]
		[Produces("application/json")]
		[ProducesResponseType<UpdateTeamProjectCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<UpdateTeamProjectCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateProject(Guid projectId, UpdateTeamProjectCommand command) {
			if (projectId != command.ProjectId) {
				return BadRequest(new UpdateTeamProjectCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The project Id Path and project Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpGet("{projectId}")]
		[Produces("application/json")]
		[ProducesResponseType<GetByIdProjectQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetByIdProjectQueryResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetProject(Guid projectId) {
			var query = new GetByIdProjectQuery {
				ProjectId = projectId
			};
			var result = await Mediator.Send(query);
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}


		[HttpDelete("{projectId}/tasks/{taskId}")]
		[Produces("application/json")]
		[ProducesResponseType<DeleteTaskCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<DeleteTaskCommandResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId) {
			var deleteProjectTaskCommand = new DeleteProjectTaskCommand() {
				ProjectId = projectId,
				TaskId = taskId
			};
			var result = await Mediator.Send(deleteProjectTaskCommand);
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		//not used
		//[HttpGet("{projectId}/taskSections")]
		//[Produces("application/json")]
		//[ProducesResponseType<GetAllProjectTaskSectionsQueryResponse>(StatusCodes.Status200OK)]
		//[ProducesResponseType<GetAllProjectTaskSectionsQueryResponse>(StatusCodes.Status404NotFound)]
		//public async Task<IActionResult> GetAllTaskSections(Guid projectId) {
		//    var result = await Mediator.Send(new GetAllProjectTaskSectionsQuery(projectId));
		//    if(!result.Success) {
		//        return NotFound(result);
		//    }
		//    return Ok(result);
		//}

		[HttpPost("{projectId}/taskSections")]
		[Produces("application/json")]
		[ProducesResponseType<CreateTaskSectionCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<CreateTaskSectionCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateTaskSection(Guid projectId, CreateTaskSectionCommand command) {
			if (projectId != command.ProjectId) {
				return BadRequest(new CreateTaskSectionCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The project Id Path and project Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpPut("{projectId}/taskSections/{section}")]
		[Produces("application/json")]
		[ProducesResponseType<RenameTaskSectionCardCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<RenameTaskSectionCardCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateTaskSection(Guid projectId, string section, RenameTaskSectionCardCommand command) {
			if (projectId != command.ProjectId) {
				return BadRequest(new RenameTaskSectionCardCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The project Id Path and project Id Body must be equal." }
				});
			}
			if (section != command.Section) {
				return BadRequest(new RenameTaskSectionCardCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The section Path and section Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpDelete("{projectId}/taskSections/{section}")]
		[Produces("application/json")]
		[ProducesResponseType<DeleteTaskSectionCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<DeleteTaskSectionCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteTaskSection(Guid projectId, string section) {
			var deleteProjectTaskSectionCommand = new DeleteTaskSectionCommand() {
				ProjectId = projectId,
				Section = section
			};
			var result = await Mediator.Send(deleteProjectTaskSectionCommand);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}

