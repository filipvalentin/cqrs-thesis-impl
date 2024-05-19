
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;


namespace Lunatic.Infrastructure.Repositories {
    public class TeamRepository : BaseRepository<Team>, ITeamRepository {
        public TeamRepository(LunaticContext context) : base(context) {
        }
    }
}

