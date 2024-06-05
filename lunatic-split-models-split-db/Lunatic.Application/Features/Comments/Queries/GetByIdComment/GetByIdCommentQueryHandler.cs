using AutoMapper;
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.ReadSide;
using MediatR;

namespace Lunatic.Application.Features.Comments.Queries.GetByIdComment {
	public class GetByIdCommentQueryHandler : IRequestHandler<GetByIdCommentQuery, GetByIdCommentQueryResponse> {
		private readonly ICommentReadSideRepository commentRepository;
		private readonly IMapper mapper;
		public GetByIdCommentQueryHandler(ICommentReadSideRepository commentRepository, IMapper mapper) {
			this.commentRepository = commentRepository;
			this.mapper = mapper;
		}

		public async Task<GetByIdCommentQueryResponse> Handle(GetByIdCommentQuery request, CancellationToken cancellationToken) {
			var commentResponse = await commentRepository.FindByIdAsync(request.CommentId);

			return new GetByIdCommentQueryResponse {
				Success = true,
				Comment = mapper.Map<CommentDto>(commentResponse.Value)
			};
		}
	}
}
