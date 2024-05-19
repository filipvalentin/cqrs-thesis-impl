using Lunatic.Application.Features.Projects;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Infrastructure.Data;

namespace Lunatic.Infrastructure.ReadServices {
	public sealed class ProjectReadService : GenericReadService<ProjectReadModel>, IProjectReadService {
		public ProjectReadService(LunaticReadContext context) : base(context) {
		}
	}
}
