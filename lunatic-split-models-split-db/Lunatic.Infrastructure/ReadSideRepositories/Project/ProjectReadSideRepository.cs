using Lunatic.Application.Contracts;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;

namespace Lunatic.Infrastructure.ReadSideRepositories.Project
{
    public class ProjectReadSideRepository : BaseReadSideRepository<ProjectReadModel>, IProjectReadSideRepository
    {
        public ProjectReadSideRepository(ILunaticReadContext context) : base(context)
        {
        }
    }
}
