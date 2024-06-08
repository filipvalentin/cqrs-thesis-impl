using Lunatic.Domain.Utils;
using Task = Lunatic.Domain.Entities.Task;

namespace Lunatic.Application.Persistence.WriteSide {
	public interface ITaskRepository : IAsyncRepository<Task> {
		public Task<Result<List<Task>>> GetAllTasksByProjectIdAndSectionCardAsync(Guid projectId, string section);
	}
}