using Lunatic.Application.Features.Tasks;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Domain.Utils;
using Lunatic.Infrastructure.Data;

namespace Lunatic.Infrastructure.ReadServices {
	public sealed class TaskReadService : GenericReadService<TaskReadModel>, ITaskReadService {

		public TaskReadService(LunaticReadContext context) : base(context) {
		}

		public async Task<Result<CompositeTaskReadModel>> GetCompositeTaskByIdAsync(Guid taskId) {
			var result = await context.CompoundTaskReadModel.FindAsync(taskId);
			//.Include(t => t.Comments)
			//.FirstOrDefaultAsync(t => t.Id == taskId);
			if (result == null) {
				return Result<CompositeTaskReadModel>.Failure($"Entity with id {taskId} not found");
			}
			return Result<CompositeTaskReadModel>.Success(result);
		}

		public async Task<Result<TaskDescriptionReadModel>> GetTaskDescriptionByIdAsync(Guid taskId) {
			var result = await context.TaskDescriptionReadModel.FindAsync(taskId);
			if (result == null) {
				return Result<TaskDescriptionReadModel>.Failure($"Entity with id {taskId} not found");
			}
			return Result<TaskDescriptionReadModel>.Success(result);
		}
	}
}
