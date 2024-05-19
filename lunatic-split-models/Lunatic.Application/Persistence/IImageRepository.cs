using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;

namespace Lunatic.Application.Persistence {
	public interface IImageRepository : IAsyncRepository<Image> {
		Task<bool> ExistsByUserIdAsync(Guid userId);
		Task<Result<Image>> FindByUserIdAsync(Guid userId);
	}
}
