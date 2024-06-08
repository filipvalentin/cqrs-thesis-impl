using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.Entities;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.AddTeamProject {
	public class CreateTeamProjectCommandHandler : IRequestHandler<CreateTeamProjectCommand, CreateTeamProjectCommandResponse> {
		private readonly IProjectRepository projectRepository;
		private readonly ITeamRepository teamRepository;
		private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public CreateTeamProjectCommandHandler(IProjectRepository projectRepository, ITeamRepository teamRepository,
			IUserRepository userRepository, IPublisher publisher, IMapper mapper) {
			this.projectRepository = projectRepository;
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<CreateTeamProjectCommandResponse> Handle(CreateTeamProjectCommand request, CancellationToken cancellationToken) {
			var validator = new CreateTeamProjectCommandValidator(userRepository, teamRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new CreateTeamProjectCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var projectResult = Project.Create(request.UserId, request.TeamId, request.Title, request.Description);
			if (!projectResult.IsSuccess) {
				return new CreateTeamProjectCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { projectResult.Error }
				};
			}

			var project = projectResult.Value;

			var team = (await teamRepository.FindByIdAsync(request.TeamId)).Value;
			team.AddProject(project.Id);
			await teamRepository.UpdateAsync(team);

			await projectRepository.AddAsync(project);

			await publisher.Publish(new ProjectCreatedDomainEvent(project.Id), cancellationToken);

			return new CreateTeamProjectCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(project)
			};
		}
	}
}
