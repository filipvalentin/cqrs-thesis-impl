using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.ReadSide.Task;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTask {
	public class GetByIdTaskQueryHandler : IRequestHandler<GetByIdTaskQuery, GetByIdTaskQueryResponse> {
		private readonly ITaskReadSideRepository taskReadService;
		private readonly IMapper mapper;

		public GetByIdTaskQueryHandler(ITaskReadSideRepository taskReadService, IMapper mapper) {
			this.taskReadService = taskReadService;
			this.mapper = mapper;
		}

		public async Task<GetByIdTaskQueryResponse> Handle(GetByIdTaskQuery request, CancellationToken cancellationToken) {
			var result = await taskReadService.FindByIdAsync(request.TaskId);

			if (!result.IsSuccess) {
				return new GetByIdTaskQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}

			return new GetByIdTaskQueryResponse {
				Success = true,
				Task = mapper.Map<TaskDto>(result.Value)
			};
		}
	}
}
