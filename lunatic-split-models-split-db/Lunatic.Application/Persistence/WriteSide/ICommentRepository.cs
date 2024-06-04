using Lunatic.Domain.Entities;


namespace Lunatic.Application.Persistence.WriteSide
{
    public interface ICommentRepository : IAsyncRepository<Comment>
    {
    }
}

