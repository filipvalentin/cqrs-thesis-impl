using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.Utils;
using Lunatic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Task = Lunatic.Domain.Entities.Task;


namespace Lunatic.Infrastructure.Repositories {
	public class TaskRepository : BaseRepository<Task>, ITaskRepository {
		public TaskRepository(LunaticContext context) : base(context) {
		}

		public async Task<Result<List<Task>>> GetAllTasksByProjectIdAndSectionCardAsync(Guid projectId, string sectionCard) {
			var tasks = await context.Tasks
				.Where(t => t.ProjectId == projectId && t.TaskSectionCard == sectionCard)
				.ToListAsync();

			return Result<List<Task>>.Success(tasks);
		}
	}
}

