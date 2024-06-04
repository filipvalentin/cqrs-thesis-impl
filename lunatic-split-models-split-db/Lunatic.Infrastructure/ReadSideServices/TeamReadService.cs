using Lunatic.Application.Contracts;
using Lunatic.Application.Features.Teams;
using Lunatic.Application.Models.ReadModels;

namespace Lunatic.Infrastructure.ReadSideServices {
	public sealed class TeamReadService : GenericReadService<TeamReadModel>, ITeamReadService {
		public TeamReadService(ILunaticReadContext context) : base(context) {
		}
	}
}
