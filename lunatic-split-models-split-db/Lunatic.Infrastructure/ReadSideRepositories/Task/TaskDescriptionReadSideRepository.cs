using Lunatic.Application.Contracts;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Application.Persistence.ReadSide.Task;

namespace Lunatic.Infrastructure.ReadSideRepositories.Task {
	public class TaskDescriptionReadSideRepository : BaseReadSideRepository<TaskDescriptionReadModel>, ITaskDescriptionReadSideRepository {
		public TaskDescriptionReadSideRepository(ILunaticReadContext context) : base(context) {
		}
	}
}
