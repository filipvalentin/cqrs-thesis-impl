using Lunatic.Domain.Utils;

namespace Lunatic.Application.Persistence.WriteSide {
	public interface IAsyncRepository<T> where T : class {
		Task<bool> ExistsByIdAsync(Guid id);
		Task<Result<T>> FindByIdAsync(Guid id);
		Task<Result<T>> AddAsync(T entity);
		Task<Result<T>> UpdateAsync(T entity);
		Task<Result<T>> DeleteAsync(Guid id);

		//Task<Result<IReadOnlyList<T>>> GetPagedReponseAsync(int page, int size);
		Task<Result<IReadOnlyList<T>>> GetAllAsync();
	}
}

