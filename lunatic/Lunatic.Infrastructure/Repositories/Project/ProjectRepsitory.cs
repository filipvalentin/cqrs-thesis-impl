
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;


namespace Lunatic.Infrastructure.Repositories {
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository {
        public ProjectRepository(LunaticContext context) : base(context) {
        }
    }
}

