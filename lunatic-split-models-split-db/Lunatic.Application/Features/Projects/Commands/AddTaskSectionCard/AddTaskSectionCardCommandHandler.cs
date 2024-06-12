using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.CreateTaskSectionCard {
	public class AddTaskSectionCardCommandHandler(
		IProjectRepository projectRepository, 
		IPublisher publisher, 
		IMapper mapper) : IRequestHandler<AddTaskSectionCardCommand, AddTaskSectionCardCommandResponse> {
		
		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;

		public async Task<AddTaskSectionCardCommandResponse> Handle(AddTaskSectionCardCommand request, CancellationToken cancellationToken) {
			var projectResult = await projectRepository.FindByIdAsync(request.ProjectId);
			if (!projectResult.IsSuccess) {
				return new AddTaskSectionCardCommandResponse {
					Success = false,
					Message = projectResult.Error
				};
			}

			var project = projectResult.Value;
			project.AddTaskSectionCard(request.Section);
			var dbProjectResult = await projectRepository.UpdateAsync(project);
			if (!dbProjectResult.IsSuccess) {
				return new AddTaskSectionCardCommandResponse {
					Success = false,
					Message = dbProjectResult.Error
				};
			}

			await publisher.Publish(mapper.Map<ProjectUpdatedDomainEvent>(project), cancellationToken);

			return new AddTaskSectionCardCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(project)
			};
		}
	}
}
