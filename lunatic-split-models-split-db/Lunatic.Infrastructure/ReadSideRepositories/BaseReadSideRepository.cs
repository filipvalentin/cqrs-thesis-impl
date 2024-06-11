
using Lunatic.Application.Contracts;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Domain.Utils;
using MongoDB.Driver;

namespace Lunatic.Infrastructure.ReadSideRepositories {
	public class BaseReadSideRepository<T> : IAsyncReadSideRepository<T> where T : class {
		protected readonly ILunaticReadContext context;
		public BaseReadSideRepository(ILunaticReadContext context) {
			this.context = context;
		}

		public async Task<Result<T>> FindByIdAsync(Guid id) {
			try {
				var collection = context.GetCollection<T>();
				var filter = Builders<T>.Filter.Eq("Id", id);
				var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
				var result = await collection.Find(filter).FirstOrDefaultAsync(cancellationTokenSource.Token);

				if (result == null) {
					return Result<T>.Failure($"Entity with id {id} not found");
				}

				return Result<T>.Success(result);
			}
			catch (Exception ex) {
				return Result<T>.Failure(ex.Message);
			}
		}

		public async Task<Result> UpdateAsync(Guid id, T updatedEntity) {
			try {
				var collection = context.GetCollection<T>();
				var filter = Builders<T>.Filter.Eq("Id", id);
				var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
				var result = await collection.ReplaceOneAsync(filter, updatedEntity, cancellationToken: cancellationTokenSource.Token);

				if (result.MatchedCount == 0) {
					return Result.Failure($"Entity with id {id} not found");
				}

				return Result.Success();
			}
			catch (Exception ex) {
				return Result.Failure(ex.Message);
			}
		}

		public async Task<Result> AddAsync(T entity) {
			try {
				var collection = context.GetCollection<T>();
				var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
				await collection.InsertOneAsync(entity, cancellationToken: cancellationTokenSource.Token);
				return Result.Success();
			}
			catch (Exception ex) {
				return Result.Failure(ex.Message);
			}
		}

		public async Task<Result> DeleteAsync(Guid id) {
			try {
				var collection = context.GetCollection<T>();
				var filter = Builders<T>.Filter.Eq("Id", id);
				var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
				var result = await collection.DeleteOneAsync(filter, cancellationToken: cancellationTokenSource.Token);

				if (result.DeletedCount == 0) {
					return Result.Failure($"Entity with id {id} not found");
				}

				return Result.Success();
			}
			catch (Exception ex) {
				return Result.Failure(ex.Message);
			}
		}
	}
}
