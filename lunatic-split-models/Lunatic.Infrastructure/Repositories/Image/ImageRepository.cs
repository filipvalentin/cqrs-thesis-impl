using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;
using Lunatic.Infrastructure.Data;

namespace Lunatic.Infrastructure.Repositories
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository {
		public ImageRepository(LunaticContext context) : base(context) { }

		public async Task<bool> ExistsByUserIdAsync(Guid userId) {
			var result = await FindByUserIdAsync(userId);
			return result.IsSuccess;
		}

		public async Task<Result<Image>> FindByUserIdAsync(Guid userId) {
			var result = await context.Set<Image>().FindAsync(userId);
			if (result == null) {
				return Result<Image>.Failure($"Entity with id {userId} not found");
			}
			return Result<Image>.Success(result);
		}
	}
}
