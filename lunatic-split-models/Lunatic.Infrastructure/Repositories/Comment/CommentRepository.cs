
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using Lunatic.Infrastructure.Data;


namespace Lunatic.Infrastructure.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository {
        public CommentRepository(LunaticContext context) : base(context) {
        }
    }
}

