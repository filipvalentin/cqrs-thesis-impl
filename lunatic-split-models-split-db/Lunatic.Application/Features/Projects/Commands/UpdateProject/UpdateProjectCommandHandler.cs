using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.UpdateProject {
	public class UpdateTeamProjectCommandHandler(
		IProjectRepository projectRepository, 
		IMapper mapper, 
		IPublisher publisher) : IRequestHandler<UpdateTeamProjectCommand, UpdateTeamProjectCommandResponse> {

		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly IMapper mapper = mapper;
		private readonly IPublisher publisher = publisher;

		public async Task<UpdateTeamProjectCommandResponse> Handle(UpdateTeamProjectCommand request, CancellationToken cancellationToken) {

			var projectResult = await projectRepository.FindByIdAsync(request.ProjectId);
			if (!projectResult.IsSuccess) {
				return new UpdateTeamProjectCommandResponse {
					Success = false,
					Message = projectResult.Error
				};
			}

			projectResult.Value.Update(request.Title, request.Description);
			var dbProjectResult = await projectRepository.UpdateAsync(projectResult.Value);
			if (!dbProjectResult.IsSuccess) {
				return new UpdateTeamProjectCommandResponse {
					Success = false,
					Message = dbProjectResult.Error
				};
			}

			await publisher.Publish(mapper.Map<ProjectUpdatedDomainEvent>(dbProjectResult.Value), cancellationToken);

			return new UpdateTeamProjectCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(dbProjectResult.Value)
			};
		}
	}
}
