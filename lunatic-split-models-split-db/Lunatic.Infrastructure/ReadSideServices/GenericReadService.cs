using Lunatic.Application.Contracts;
using Lunatic.Application.Contracts.Interfaces;
using Lunatic.Domain.Utils;
using MongoDB.Driver;

namespace Lunatic.Infrastructure.ReadSideServices {
	public class GenericReadService<T> : IGenericReadService<T> where T : class {
		protected readonly ILunaticReadContext context;

		public GenericReadService(ILunaticReadContext context) {
			this.context = context;
		}

		public async Task<Result<T>> GetByIdAsync(Guid id) {
			var collection = context.GetCollection<T>();
			var filter = Builders<T>.Filter.Eq("Id", id);
			var result = await collection.Find(filter).FirstOrDefaultAsync();

			if (result == null) {
				return Result<T>.Failure($"Entity with id {id} not found");
			}

			return Result<T>.Success(result);
		}
	}
}
