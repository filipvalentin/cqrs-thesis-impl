using AutoMapper;
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.ReadSide;
using MediatR;

namespace Lunatic.Application.Features.Comments.Queries.GetByIdComment {
	public class GetByIdCommentQueryHandler(ICommentReadSideRepository commentRepository, IMapper mapper) : IRequestHandler<GetByIdCommentQuery, GetByIdCommentQueryResponse> {
		private readonly ICommentReadSideRepository commentRepository = commentRepository;
		private readonly IMapper mapper = mapper;

		public async Task<GetByIdCommentQueryResponse> Handle(GetByIdCommentQuery request, CancellationToken cancellationToken) {
			var commentResponse = await commentRepository.FindByIdAsync(request.CommentId);
			if (!commentResponse.IsSuccess) {
				return new GetByIdCommentQueryResponse {
					Success = false,
					Message = commentResponse.Error
				};
			}

			return new GetByIdCommentQueryResponse {
				Success = true,
				Comment = mapper.Map<CommentDto>(commentResponse.Value)
			};
		}
	}
}
