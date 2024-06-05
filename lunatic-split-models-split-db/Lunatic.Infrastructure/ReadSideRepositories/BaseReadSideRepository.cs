
using Lunatic.Application.Contracts;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Domain.Utils;
using MongoDB.Driver;

namespace Lunatic.Infrastructure.ReadSideRepositories {
	public class BaseReadSideRepository<T> : IAsyncReadSideRepository<T> where T : class{
		protected readonly ILunaticReadContext context;
		public BaseReadSideRepository(ILunaticReadContext context) {
			this.context = context;
		}

		public async Task<Result<T>> FindByIdAsync(Guid id) {
			var collection = context.GetCollection<T>();
			var filter = Builders<T>.Filter.Eq("Id", id);
			var result = await collection.Find(filter).FirstOrDefaultAsync();

			if (result == null) {
				return Result<T>.Failure($"Entity with id {id} not found");
			}

			return Result<T>.Success(result);
		}

		public async Task<Result> UpdateAsync(Guid id, T updatedEntity) {
			var collection = context.GetCollection<T>();
			var filter = Builders<T>.Filter.Eq("Id", id);
			var result = await collection.ReplaceOneAsync(filter, updatedEntity);

			if (result.MatchedCount == 0) {
				return Result.Failure($"Entity with id {id} not found");
			}

			return Result.Success();
		}

		public async Task<Result> AddAsync(T entity) {
			var collection = context.GetCollection<T>();
			await collection.InsertOneAsync(entity);
			return Result.Success();
		}

		public async Task<Result> DeleteAsync(Guid id) {
			var collection = context.GetCollection<T>();
			var filter = Builders<T>.Filter.Eq("Id", id);
			var result = await collection.DeleteOneAsync(filter);

			if (result.DeletedCount == 0) {
				return Result.Failure($"Entity with id {id} not found");
			}

			return Result.Success();
		}
	}
}
