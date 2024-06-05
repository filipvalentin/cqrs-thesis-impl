using Lunatic.Application.Models.ReadModels;
using Lunatic.Domain.Utils;

namespace Lunatic.Application.Persistence.ReadSide {
	public interface ITeamReadSideRepository : IAsyncReadSideRepository<TeamReadModel> {
		public Task<Result<List<UserReadModel>>> GetTeamMembers(Guid teamId);
	}
}
