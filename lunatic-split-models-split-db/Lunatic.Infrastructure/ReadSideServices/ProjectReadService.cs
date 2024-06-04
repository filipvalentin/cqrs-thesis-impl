using Lunatic.Application.Contracts;
using Lunatic.Application.Features.Projects;
using Lunatic.Application.Models.ReadModels;

namespace Lunatic.Infrastructure.ReadSideServices {
	public sealed class ProjectReadService : GenericReadService<ProjectReadModel>, IProjectReadService {
		public ProjectReadService(ILunaticReadContext context) : base(context) {
		}
	}
}
