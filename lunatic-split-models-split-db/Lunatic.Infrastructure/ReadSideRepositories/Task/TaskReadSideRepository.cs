using Lunatic.Application.Contracts;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Application.Persistence.ReadSide.Task;

namespace Lunatic.Infrastructure.ReadSideRepositories.Task
{
    public class TaskReadSideRepository : BaseReadSideRepository<TaskReadModel>, ITaskReadSideRepository {
		public TaskReadSideRepository(ILunaticReadContext context) : base(context) {
		}
	}
}
