
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;
using Lunatic.Infrastructure.Data;


namespace Lunatic.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository {
        public UserRepository(LunaticContext context) : base(context) {
        }

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
            if(result.IsSuccess) {
                List<User> users = result.Value.ToList();
                User? expectedUser = users.Find(user => user.Username == username);

                if(expectedUser == null) {
                    return Result<User>.Failure($"Entity with username {username} not found");
                }
                return Result<User>.Success(expectedUser);
            }
            return Result<User>.Failure($"Entity with username {username} not found");
        }

        public async Task<Result<User>> FindByEmailAsync(string email) {
            var result = await GetAllAsync();
            if(result.IsSuccess) {
                List<User> users = result.Value.ToList();
                User? expectedUser = users.Find(user => user.Email == email);

                if(expectedUser == null) {
                    return Result<User>.Failure($"Entity with email {email} not found");
                }
                return Result<User>.Success(expectedUser);
            }
            return Result<User>.Failure($"Entity with email {email} not found");
        }
    }
}

