using Task = Lunatic.Domain.Entities.Task;


namespace Lunatic.Application.Persistence.WriteSide
{
    public interface ITaskRepository : IAsyncRepository<Task>
    {
    }
}

