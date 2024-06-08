using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;
using Task = Lunatic.Domain.Entities.Task;

namespace Lunatic.Application.Features.Projects.Commands.CreateTask {
	public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskCommandResponse> {
		private readonly ITaskRepository taskRepository;
		private readonly IProjectRepository projectRepository;
		private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public CreateTaskCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, 
			IUserRepository userRepository, IPublisher publisher, IMapper mapper) {
			this.taskRepository = taskRepository;
			this.projectRepository = projectRepository;
			this.userRepository = userRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<CreateTaskCommandResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken) {
			var validator = new CreateTaskCommandValidator(userRepository, projectRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new CreateTaskCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var taskResult = Task.Create(request.UserId, request.ProjectId, request.Section, request.Title, request.Description, request.Priority, request.PlannedStartDate, request.PlannedEndDate);
			if (!taskResult.IsSuccess) {
				return new CreateTaskCommandResponse {
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

			return new CreateTaskCommandResponse {
				Success = true,
				Task = mapper.Map<TaskDto>(taskResult.Value)
			};
		}
	}
}
