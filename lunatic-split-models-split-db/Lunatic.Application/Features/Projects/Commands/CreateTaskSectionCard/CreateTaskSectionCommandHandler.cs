using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.CreateTaskSectionCard {
	public class CreateTaskSectionCommandHandler : IRequestHandler<CreateTaskSectionCommand, CreateTaskSectionCommandResponse> {
		private readonly IProjectRepository projectRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public CreateTaskSectionCommandHandler(IProjectRepository projectRepository, IPublisher publisher, IMapper mapper) {
			this.projectRepository = projectRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<CreateTaskSectionCommandResponse> Handle(CreateTaskSectionCommand request, CancellationToken cancellationToken) {
			//var validator = new CreateTaskSectionCommandValidator(projectReadRepository);
			//var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			//if (!validatorResult.IsValid) {
			//	return new CreateTaskSectionCommandResponse {
			//		Success = false,
			//		ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
			//	};
			//}

			var project = (await projectRepository.FindByIdAsync(request.ProjectId)).Value;
			project.AddTaskSectionCard(request.Section);
			var dbProjectResult = await projectRepository.UpdateAsync(project);

			return new CreateTaskSectionCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(dbProjectResult.Value)
			};
		}
	}
}
