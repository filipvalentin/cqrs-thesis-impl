using Lunatic.Application.Models.ReadModels.Tasks;

namespace Lunatic.Application.Persistence.ReadSide.Task
{
    public interface ITaskReadSideRepository : IAsyncReadSideRepository<TaskReadModel>
    {
    }
}
