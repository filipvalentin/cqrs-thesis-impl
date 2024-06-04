using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;

namespace Lunatic.Application.Persistence.WriteSide
{
    public interface IImageRepository : IAsyncRepository<Image>
    {
        Task<bool> ExistsByUserIdAsync(Guid userId);
        Task<Result<Image>> FindByUserIdAsync(Guid userId);
    }
}
