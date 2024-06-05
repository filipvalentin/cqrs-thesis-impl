using Lunatic.Application.Contracts;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;

namespace Lunatic.Infrastructure.ReadSideRepositories.Team
{
    public class TeamReadSideRepository : BaseReadSideRepository<TeamReadModel>, ITeamReadSideRepository
    {
        public TeamReadSideRepository(ILunaticReadContext context) : base(context)
        {
        }
    }
}
