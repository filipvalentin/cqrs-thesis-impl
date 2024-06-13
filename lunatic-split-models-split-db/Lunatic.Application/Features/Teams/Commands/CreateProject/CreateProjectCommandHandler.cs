using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.DomainEvents.Team;
using Lunatic.Domain.Entities;
using MediatR;


namespace Lunatic.Application.Features.Teams.Commands.CreateProject {
	public class CreateProjectCommandHandler(
		IProjectRepository projectRepository,
		ITeamRepository teamRepository,
		IUserRepository userRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<CreateProjectCommand, CreateProjectCommandResponse> {

		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly ITeamRepository teamRepository = teamRepository;
		private readonly IUserRepository userRepository = userRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<CreateProjectCommandResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken) {
			var projectResult = Project.Create(request.UserId, request.TeamId, request.Title, request.Description);
			if (!projectResult.IsSuccess) {
				return new CreateProjectCommandResponse {
					Success = false,
					Message = projectResult.Error
				};
			}
			var project = projectResult.Value;

			var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
			if (!teamResult.IsSuccess) {
				return new CreateProjectCommandResponse {
					Success = false,
					Message = teamResult.Error
				};
			}
			var team = teamResult.Value;
			team.AddProject(project.Id);
			var teamUpdatedResult = await teamRepository.UpdateAsync(team);
			if (!teamUpdatedResult.IsSuccess) {
				return new CreateProjectCommandResponse {
					Success = false,
					Message = teamUpdatedResult.Error
				};
			}

			var projectCreatedResult = await projectRepository.AddAsync(project);
			if (!projectCreatedResult.IsSuccess) {
				return new CreateProjectCommandResponse {
					Success = false,
					Message = projectCreatedResult.Error
				};
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<ProjectCreatedDomainEvent>(project), cancellationToken);
			await publisher.Publish(mapper.Map<TeamUpdatedDomainEvent>(team), cancellationToken);

			return new CreateProjectCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(project)
			};
		}
	}
}
