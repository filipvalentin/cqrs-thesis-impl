using Lunatic.Application.Contracts;
using Lunatic.Application.Features.Users.Queries.GetUsernameMatches;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lunatic.Infrastructure.ReadSideRepositories.User {
	public class UserReadSideRepository : BaseReadSideRepository<UserReadModel>, IUserReadSideRepository {
		public UserReadSideRepository(ILunaticReadContext context) : base(context) {
		}

		public async Task<List<UsernameMatch>> GetUsernameMatches(string startingWith, int take = 10) {

			var filter = Builders<UserReadModel>.Filter.Regex("Username", new BsonRegularExpression($"^{startingWith}", "i"));

			var result = await context.GetCollection<UserReadModel>()
				.Find(filter)
				.Limit(take)
				.Project(user => new UsernameMatch {
					Username = user.Username,
					UserId = user.Id
				})
				.ToListAsync();

			return result;
		}
	}
}
