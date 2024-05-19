using Lunatic.Application.Features.Teams;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Infrastructure.Data;

namespace Lunatic.Infrastructure.ReadServices {
	public sealed class TeamReadService : GenericReadService<TeamReadModel>, ITeamReadService {
		public TeamReadService(LunaticReadContext context) : base(context) {
		}
	}
}
