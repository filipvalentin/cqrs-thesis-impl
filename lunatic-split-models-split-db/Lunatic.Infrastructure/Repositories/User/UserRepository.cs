using Lunatic.Application.Contracts.Identity;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;
using Lunatic.Infrastructure.Data;


namespace Lunatic.Infrastructure.Repositories {
	public class UserRepository(
		LunaticContext context,
		IAuthService authService) : BaseRepository<User>(context), IUserRepository {

		private readonly IAuthService authService = authService;

		public async Task<bool> ExistsByUsernameAsync(string username) {
			var result = await FindByUsernameAsync(username);
			return result.IsSuccess;
		}

		public async Task<bool> ExistsByEmailAsync(string email) {
			var result = await FindByEmailAsync(email);
			return result.IsSuccess;
		}

		public async Task<Result<User>> FindByUsernameAsync(string username) {
			var result = await GetAllAsync();
			if (result.IsSuccess) {
				List<User> users = result.Value.ToList();
				User? expectedUser = users.Find(user => user.Username == username);

				if (expectedUser == null) {
					return Result<User>.Failure($"Entity with username {username} not found");
				}
				return Result<User>.Success(expectedUser);
			}
			return Result<User>.Failure($"Entity with username {username} not found");
		}

		public async Task<Result<User>> FindByEmailAsync(string email) {
			var result = await GetAllAsync();
			if (result.IsSuccess) {
				List<User> users = result.Value.ToList();
				User? expectedUser = users.Find(user => user.Email == email);

				if (expectedUser == null) {
					return Result<User>.Failure($"Entity with email {email} not found");
				}
				return Result<User>.Success(expectedUser);
			}
			return Result<User>.Failure($"Entity with email {email} not found");
		}

		public override async Task<Result<User>> DeleteAsync(Guid id) {
			var userUregistered = await authService.Unregister(id);
			if(!userUregistered) {
				return Result<User>.Failure($"Failed to unregister user with id {id}");
			}
			return await base.DeleteAsync(id);
		}
	}
}

