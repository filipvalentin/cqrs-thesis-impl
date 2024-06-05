using Lunatic.Application.Features.Users.Queries.GetUsernameMatches;
using Lunatic.Application.Models.ReadModels;

namespace Lunatic.Application.Persistence.ReadSide {
	public interface IUserReadSideRepository : IAsyncReadSideRepository<UserReadModel> {
		public Task<List<UsernameMatch>> GetUsernameMatches(string startingWith, int take = 10);
	}
}
