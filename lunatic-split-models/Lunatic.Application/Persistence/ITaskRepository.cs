
using Task = Lunatic.Domain.Entities.Task;


namespace Lunatic.Application.Persistence {
    public interface ITaskRepository : IAsyncRepository<Task> {
    }
}

