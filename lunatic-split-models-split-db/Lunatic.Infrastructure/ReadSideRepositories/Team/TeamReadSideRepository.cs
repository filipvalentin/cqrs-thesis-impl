using Lunatic.Application.Contracts;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Domain.Utils;
using MongoDB.Driver;

namespace Lunatic.Infrastructure.ReadSideRepositories.Team {
	public class TeamReadSideRepository(ILunaticReadContext context) : BaseReadSideRepository<TeamReadModel>(context), ITeamReadSideRepository {
		public async Task<Result<List<UserReadModel>>> GetTeamMembers(Guid teamId) {
			var filter = Builders<TeamReadModel>.Filter.Eq(restaurant => restaurant.Id, teamId);

			var team = await DbSet.Find(filter).FirstOrDefaultAsync();
			if (team == null) {
				return Result<List<UserReadModel>>.Failure($"Team with id {teamId} not found");
			}

			var userFilter = Builders<UserReadModel>.Filter.In(u => u.Id, team.MemberIds);
			var users = await context.GetCollection<UserReadModel>().Find(userFilter).ToListAsync();

			return Result<List<UserReadModel>>.Success(users);
		}
	}
}
