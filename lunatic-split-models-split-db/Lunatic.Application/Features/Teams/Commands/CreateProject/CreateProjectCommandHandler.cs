using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.Entities;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.CreateProject {
	public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CreateProjectCommandResponse> {
		private readonly IProjectRepository projectRepository;
		private readonly ITeamRepository teamRepository;
		private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public CreateProjectCommandHandler(IProjectRepository projectRepository, ITeamRepository teamRepository,
			IUserRepository userRepository, IPublisher publisher, IMapper mapper) {
			this.projectRepository = projectRepository;
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<CreateProjectCommandResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken) {
			var projectResult = Project.Create(request.UserId, request.TeamId, request.Title, request.Description);
			if (!projectResult.IsSuccess) {
				return new CreateProjectCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { projectResult.Error }
				};
			}

			var project = projectResult.Value;

			var team = (await teamRepository.FindByIdAsync(request.TeamId)).Value;
			team.AddProject(project.Id);
			await teamRepository.UpdateAsync(team);

			await projectRepository.AddAsync(project);

			await publisher.Publish(mapper.Map<ProjectCreatedDomainEvent>(project), cancellationToken);

			return new CreateProjectCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(project)
			};
		}
	}
}
