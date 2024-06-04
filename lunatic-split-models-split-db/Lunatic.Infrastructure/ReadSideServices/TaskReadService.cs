using Lunatic.Application.Contracts;
using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Domain.Utils;
using MongoDB.Driver;

namespace Lunatic.Infrastructure.ReadSideServices {
	public sealed class TaskReadService : GenericReadService<TaskReadModel>, ITaskReadService {

		public TaskReadService(ILunaticReadContext context) : base(context) {
		}

		public async Task<Result<CompositeTaskReadModel>> GetCompositeTaskByIdAsync(Guid taskId) {
			var collection = context.GetCollection<CompositeTaskReadModel>();
			var filter = Builders<CompositeTaskReadModel>.Filter.Eq("Id", taskId);
			var result = await collection.Find(filter).FirstOrDefaultAsync();

			if (result == null) {
				return Result<CompositeTaskReadModel>.Failure($"Entity with id {taskId} not found");
			}
			return Result<CompositeTaskReadModel>.Success(result);
		}

		public async Task<Result<TaskDescriptionReadModel>> GetTaskDescriptionByIdAsync(Guid taskId) {
			var collection = context.GetCollection<TaskDescriptionReadModel>();
			var filter = Builders<TaskDescriptionReadModel>.Filter.Eq("Id", taskId);
			var result = await collection.Find(filter).FirstOrDefaultAsync();

			if (result == null) {
				return Result<TaskDescriptionReadModel>.Failure($"Entity with id {taskId} not found");
			}
			return Result<TaskDescriptionReadModel>.Success(result);
		}
	}
}
