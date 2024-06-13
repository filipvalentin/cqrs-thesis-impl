using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;
using Task = Lunatic.Domain.Entities.Task;

namespace Lunatic.Application.Features.Projects.Commands.CreateTask {
	public class CreateTaskCommandHandler(
		ITaskRepository taskRepository,
		IProjectRepository projectRepository,
		IUserRepository userRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskCommand, CreateTaskCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly IUserRepository userRepository = userRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<CreateTaskCommandResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken) {

			var taskResult = Task.Create(request.UserId, request.ProjectId, request.Section, request.Title, request.Description, request.Priority, request.PlannedStartDate, request.PlannedEndDate);
			if (!taskResult.IsSuccess) {
				return new CreateTaskCommandResponse {
					Success = false,
					Message = taskResult.Error
				};
			}

			var task = taskResult.Value;

			var projectResult = await projectRepository.FindByIdAsync(request.ProjectId);
			if (!projectResult.IsSuccess) {
				return new CreateTaskCommandResponse {
					Success = false,
					Message = projectResult.Error
				};
			}
			var project = projectResult.Value;

			project.AddTask(task);

			var projectUpdatedResult = await projectRepository.UpdateAsync(project);
			if (!projectUpdatedResult.IsSuccess) {
				return new CreateTaskCommandResponse {
					Success = false,
					Message = projectUpdatedResult.Error
				};
			}

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
					Message = addResult.Error
				};
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<TaskCreatedDomainEvent>(task), cancellationToken);
			await publisher.Publish(mapper.Map<ProjectUpdatedDomainEvent>(project), cancellationToken);

			return new CreateTaskCommandResponse {
				Success = true,
				Task = mapper.Map<TaskDto>(task)
			};
		}
	}
}
