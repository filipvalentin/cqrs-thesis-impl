using Lunatic.Application.Features.Tasks;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Domain.Utils;
using Lunatic.Infrastructure.Data;

namespace Lunatic.Infrastructure.ReadServices {
	public sealed class TaskReadService : GenericReadService<TaskReadModel>, ITaskReadService {

		public TaskReadService(LunaticReadContext context) : base(context) {
		}

		public async Task<Result<CompoundTaskReadModel>> GetCompoundTaskByIdAsync(Guid taskId) {
			var result = context.CompoundTaskReadModel.FindAsync(taskId);
			if (result.Result == null) {
				return Result<CompoundTaskReadModel>.Failure($"Entity with id {taskId} not found");
			}
			return Result<CompoundTaskReadModel>.Success(result.Result);
		}
	}
}
