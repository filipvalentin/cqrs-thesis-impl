using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.ReadSide.Task;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTask {
	public class GetByIdTaskQueryHandler(ITaskReadSideRepository taskReadService, IMapper mapper)
		: IRequestHandler<GetByIdTaskQuery, GetByIdTaskQueryResponse> {

		private readonly ITaskReadSideRepository taskReadService = taskReadService;
		private readonly IMapper mapper = mapper;

		public async Task<GetByIdTaskQueryResponse> Handle(GetByIdTaskQuery request, CancellationToken cancellationToken) {
			var result = await taskReadService.FindByIdAsync(request.TaskId);
			if (!result.IsSuccess) {
				return new GetByIdTaskQueryResponse {
					Success = false,
					Message = result.Error
				};
			}

			return new GetByIdTaskQueryResponse {
				Success = true,
				Task = mapper.Map<TaskDto>(result.Value)
			};
		}
	}
}
