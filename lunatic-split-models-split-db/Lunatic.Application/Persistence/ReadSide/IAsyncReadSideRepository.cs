using Lunatic.Domain.Utils;

namespace Lunatic.Application.Persistence.ReadSide {
	public interface IAsyncReadSideRepository<T> where T : class {
		Task<Result<T>> GetByIdAsync(Guid id);
		Task<Result> Update(Guid id, T entity);
		Task<Result> AddAsync(T entity);
		Task<Result> DeleteAsync(Guid id);
	}
}
