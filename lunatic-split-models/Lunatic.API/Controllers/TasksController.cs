
using Lunatic.API.Interfaces;
using Lunatic.Application.Features.Tasks.Commands.CreateComment;
using Lunatic.Application.Features.Tasks.Commands.DeleteComment;
using Lunatic.Application.Features.Tasks.Commands.UpdateTask;
using Lunatic.Application.Features.Tasks.Commands.UpdateTaskSectionCardLocation;
using Lunatic.Application.Features.Tasks.Commands.UpdateTasksSection;
using Lunatic.Application.Features.Tasks.Commands.UpdateTaskStatus;
using Lunatic.Application.Features.Tasks.Queries.GetByIdCompositeTask;
using Lunatic.Application.Features.Tasks.Queries.GetByIdTask;
using Lunatic.Application.Features.Tasks.Queries.GetByIdTaskDescription;
using Lunatic.Application.Features.Tasks.Queries.GetPredictedTaskTime;
using Microsoft.AspNetCore.Mvc;

namespace Lunatic.API.Controllers {
	public class TasksController : BaseApiController {
		IDayConversionService _dayConversionService;

		public TasksController(IDayConversionService dayConversionService) {
			_dayConversionService = dayConversionService;
		}

		[HttpPost("{taskId}/comments")]
		[Produces("application/json")]
		[ProducesResponseType<CreateCommentCommandResponse>(StatusCodes.Status201Created)]
		[ProducesResponseType<CreateCommentCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateComment(Guid taskId, CreateCommentCommand command) {
			if (taskId != command.TaskId) {
				return BadRequest(new CreateCommentCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The task Id Path and task Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpPut("{taskId}")]
		[Produces("application/json")]
		[ProducesResponseType<UpdateTaskCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<UpdateTaskCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateTask(Guid taskId, UpdateTaskCommand command) {
			if (taskId != command.TaskId) {
				return BadRequest(new UpdateTaskCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The task Id Path and task Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpPut("{taskId}/sectionLocation")]
		[Produces("application/json")]
		[ProducesResponseType<UpdateTaskSectionCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<UpdateTaskSectionCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateSection(Guid taskId, UpdateTaskSectionCommand command) {
			if (taskId != command.ProjectId) {
				return BadRequest(new UpdateTaskSectionCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The task Id Path and task Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpGet("{taskId}")]
		[Produces("application/json")]
		[ProducesResponseType<GetByIdTaskQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetByIdTaskQueryResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByIdTask(Guid taskId) {
			var result = await Mediator.Send(new GetByIdTaskQuery {
				TaskId = taskId
			});
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		[HttpGet("{taskId}/flat")]
		[Produces("application/json")]
		[ProducesResponseType<GetByIdCompositeTaskQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetByIdCompositeTaskQueryResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByIdCompositeTask(Guid taskId) {//TODO: check taskId nonnull
			var result = await Mediator.Send(new GetByIdCompositeTaskQuery {
				TaskId = taskId
			});
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}
		[HttpGet("{taskId}/description")]
		[Produces("application/json")]
		[ProducesResponseType<GetByIdTaskDescriptionQueryResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetByIdTaskDescriptionQueryResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByIdTaskDescription(Guid taskId) {//TODO: check taskId nonnull
			var result = await Mediator.Send(new GetByIdTaskDescriptionQuery {
				TaskId = taskId
			});
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}

		[HttpPut("{taskId}/status")]
		[Produces("application/json")]
		[ProducesResponseType<UpdateTaskStatusCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<UpdateTaskStatusCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateStatus(Guid taskId, UpdateTaskStatusCommand command) {
			if (taskId != command.TaskId) {
				return BadRequest(new UpdateTaskStatusCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "The task Id Path and task Id Body must be equal." }
				});
			}

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}


		[HttpDelete("{taskId}/comments/{commentId}")]
		[Produces("application/json")]
		[ProducesResponseType<DeleteCommentCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<DeleteCommentCommandResponse>(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteComment(Guid taskId, Guid commentId) {
			var deleteTaskCommentCommand = new DeleteCommentCommand() {
				TaskId = taskId,
				CommentId = commentId
			};
			var result = await Mediator.Send(deleteTaskCommentCommand);
			if (!result.Success) {
				return NotFound(result);
			}
			return Ok(result);
		}


		[HttpGet("{taskId}/prediction")]
		[Produces("application/json")]
		[ProducesResponseType<GetPredictedTaskTimeCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetPredictedTaskTimeCommandResponse>(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetTaskTimePrediction(Guid taskId) {
			var command = new GetPredictedTaskTimeCommand() {
				TaskId = taskId
			};

			var result = await Mediator.Send(command);
			if (!result.Success) {
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpGet("convertDays/{number}")]
		[Produces("application/json")]
		[ProducesResponseType<GetPredictedTaskTimeCommandResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<GetPredictedTaskTimeCommandResponse>(StatusCodes.Status400BadRequest)]
		public IActionResult ConvertDays(int number) {
			var result = _dayConversionService.ConvertToString(number);
			return Ok(result);
		}
	}
}

