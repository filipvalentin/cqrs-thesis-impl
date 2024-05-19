using Lunatic.Application.Features.Comments;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Infrastructure.Data;

namespace Lunatic.Infrastructure.ReadServices {
	public sealed class CommentReadService : GenericReadService<CommentReadModel>, ICommentReadService {
		public CommentReadService(LunaticReadContext context) : base(context) {
		}
	}
}
