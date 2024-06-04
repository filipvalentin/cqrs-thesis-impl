using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.Entities;
using Lunatic.Infrastructure.Data;


namespace Lunatic.Infrastructure.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository {
        public TeamRepository(LunaticContext context) : base(context) {
        }
    }
}

