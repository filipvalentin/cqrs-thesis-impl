using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.Entities;
using Lunatic.Infrastructure.Data;


namespace Lunatic.Infrastructure.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository {
        public ProjectRepository(LunaticContext context) : base(context) {
        }
    }
}

