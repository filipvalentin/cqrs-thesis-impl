using Lunatic.Application.Contracts;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;

namespace Lunatic.Infrastructure.ReadSideRepositories.Comment {
	public class CommentReadSideRepository : BaseReadSideRepository<CommentReadModel>, ICommentReadSideRepository {
		public CommentReadSideRepository(ILunaticReadContext context) : base(context) {
		}
	}
}
