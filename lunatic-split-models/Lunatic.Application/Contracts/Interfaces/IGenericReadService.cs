using Lunatic.Domain.Utils;

namespace Lunatic.Application.Contracts.Interfaces {
	public interface IGenericReadService<T> where T : class {
		Task<Result<T>> GetByIdAsync(Guid id);
	}
}
