using Lunatic.Application.Features.Tasks;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Infrastructure.Data;

namespace Lunatic.Infrastructure.ReadServices {
	public sealed class TaskReadService : GenericReadService<TaskReadModel>, ITaskReadService {
		public TaskReadService(LunaticReadContext context) : base(context) {
		}
	}
}
