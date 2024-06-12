using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;
using Task = Lunatic.Domain.Entities.Task;

namespace Lunatic.Application.Features.Projects.Commands.CreateTask {
	public class CreateTaskCommandHandler(
		ITaskRepository taskRepository,
		IProjectRepository projectRepository,
		IUserRepository userRepository,
		IPublisher publisher,
		IMapper mapper) : IRequestHandler<CreateTaskCommand, CreateTaskCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly IUserRepository userRepository = userRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;

		public async Task<CreateTaskCommandResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken) {

			var taskResult = Task.Create(request.UserId, request.ProjectId, request.Section, request.Title, request.Description, request.Priority, request.PlannedStartDate, request.PlannedEndDate);
			if (!taskResult.IsSuccess) {
				return new CreateTaskCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { taskResult.Error }
				};
			}
			var task = taskResult.Value;
			var project = (await projectRepository.FindByIdAsync(request.ProjectId)).Value;
			project.AddTask(task);
			await projectRepository.UpdateAsync(project);

			foreach (var tag in request.Tags) {
				task.AddTag(tag);
			}
			foreach (var assignee in request.AssigneeIds) {
				task.AddAssignee(assignee);
			}

			var addResult = await taskRepository.AddAsync(task);
			if (!addResult.IsSuccess) {
				return new CreateTaskCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { addResult.Error }
				};
			}

			await publisher.Publish(mapper.Map<TaskCreatedDomainEvent>(task), cancellationToken);

			return new CreateTaskCommandResponse {
				Success = true,
				Task = mapper.Map<TaskDto>(task)
			};
		}
	}
}
