using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence;
using MediatR;
using Task = Lunatic.Domain.Entities.Task;


namespace Lunatic.Application.Features.Projects.Commands.CreateProjectTask {
	public class CreateProjectTaskCommandHandler : IRequestHandler<CreateProjectTaskCommand, CreateProjectTaskCommandResponse> {
		private readonly ITaskRepository taskRepository;

		private readonly IProjectRepository projectRepository;

		private readonly IUserRepository userRepository;

		public CreateProjectTaskCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IUserRepository userRepository) {
			this.taskRepository = taskRepository;
			this.projectRepository = projectRepository;
			this.userRepository = userRepository;
		}

		public async Task<CreateProjectTaskCommandResponse> Handle(CreateProjectTaskCommand request, CancellationToken cancellationToken) {
			var validator = new CreateProjectTaskCommandValidator(userRepository, projectRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new CreateProjectTaskCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var taskResult = Task.Create(request.UserId, request.ProjectId, request.Section, request.Title, request.Description, request.Priority, request.PlannedStartDate, request.PlannedEndDate);
			if (!taskResult.IsSuccess) {
				return new CreateProjectTaskCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { taskResult.Error }
				};
			}

			var project = (await projectRepository.FindByIdAsync(request.ProjectId)).Value;
			project.AddTask(taskResult.Value);
			await projectRepository.UpdateAsync(project);

			foreach (var tag in request.Tags) {
				taskResult.Value.AddTag(tag);
			}

			foreach (var assignee in request.AssigneeIds) {
				taskResult.Value.AddAssignee(assignee);
			}

			await taskRepository.AddAsync(taskResult.Value);

			return new CreateProjectTaskCommandResponse {
				Success = true,
				Task = new TaskDto {
					CreatedByUserId = taskResult.Value.CreatedByUserId,
					TaskId = taskResult.Value.Id,
					ProjectId = taskResult.Value.ProjectId,

					Section = taskResult.Value.TaskSectionCard,

					Title = taskResult.Value.Title,
					Description = taskResult.Value.Description,
					Priority = taskResult.Value.Priority,
					Status = taskResult.Value.Status,

					Tags = taskResult.Value.Tags,
					CommentIds = taskResult.Value.CommentIds,
					AssigneeIds = taskResult.Value.AssigneeIds,

					PlannedStartDate = taskResult.Value.PlannedStartDate,
					PlannedEndDate = taskResult.Value.PlannedEndDate,
					StartedDate = taskResult.Value.StartedDate,
					EndedDate = taskResult.Value.EndedDate,
				}
			};
		}
	}
}
