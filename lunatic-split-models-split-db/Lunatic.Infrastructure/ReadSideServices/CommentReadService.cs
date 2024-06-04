using Lunatic.Application.Contracts;
using Lunatic.Application.Features.Comments;
using Lunatic.Application.Models.ReadModels;

namespace Lunatic.Infrastructure.ReadSideServices {
	public sealed class CommentReadService : GenericReadService<CommentReadModel>, ICommentReadService {
		public CommentReadService(ILunaticReadContext context) : base(context) {
		}
	}
}
