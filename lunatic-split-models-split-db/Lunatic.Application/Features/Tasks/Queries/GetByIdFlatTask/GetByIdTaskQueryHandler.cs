using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.ReadSide.Task;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Queries.GetByIdFlatTask {
	public class GetByIdFlatTaskQueryHandler(IFlatTaskReadSideRepository flatTaskReadService, IMapper mapper)
		: IRequestHandler<GetByIdFlatTaskQuery, GetByIdFlatTaskQueryResponse> {

		private readonly IFlatTaskReadSideRepository flatTaskReadService = flatTaskReadService;
		private readonly IMapper mapper = mapper;

		public async Task<GetByIdFlatTaskQueryResponse> Handle(GetByIdFlatTaskQuery request, CancellationToken cancellationToken) {
			var result = await flatTaskReadService.FindByIdAsync(request.TaskId);
			if (!result.IsSuccess) {
				return new GetByIdFlatTaskQueryResponse() {
					Success = false,
					Message = result.Error
				};
			}

			return new GetByIdFlatTaskQueryResponse() {
				Success = true,
				Task = mapper.Map<FlatTaskDto>(result.Value)
			};


		}
	}
}
