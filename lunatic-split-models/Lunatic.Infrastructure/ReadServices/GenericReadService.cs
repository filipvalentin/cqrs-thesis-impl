using Lunatic.Application.Contracts.Interfaces;
using Lunatic.Domain.Utils;
using Lunatic.Infrastructure.Data;

namespace Lunatic.Infrastructure.ReadServices {
	public class GenericReadService<T> : IGenericReadService<T> where T : class {

		private readonly LunaticReadContext context;

		public GenericReadService(LunaticReadContext context) {
			this.context = context;
		}

		public async Task<Result<T>> GetByIdAsync(Guid id) {
			var result = await context.Set<T>().FindAsync(id);
			if (result == null) {
				return Result<T>.Failure($"Entity with id {id} not found");
			}
			return Result<T>.Success(result);
		}
	}
}
