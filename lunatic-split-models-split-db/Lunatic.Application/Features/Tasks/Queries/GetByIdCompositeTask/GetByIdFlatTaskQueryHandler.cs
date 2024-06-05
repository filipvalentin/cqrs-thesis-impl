using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.ReadSide.Task;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdCompositeTask {
	public class GetByIdFlatTaskQueryHandler : IRequestHandler<GetByIdFlatTaskQuery, GetByIdFlatTaskQueryResponse> {
		private readonly ITaskReadSideRepository taskReadRepository;
		private readonly IMapper mapper;

		public GetByIdFlatTaskQueryHandler(ITaskReadSideRepository taskReadRepository, IMapper mapper) {
			this.taskReadRepository = taskReadRepository;
			this.mapper = mapper;
		}

		public async Task<GetByIdFlatTaskQueryResponse> Handle(GetByIdFlatTaskQuery request, CancellationToken cancellationToken) {
			var result = await taskReadRepository.FindByIdAsync(request.TaskId);

			if (!result.IsSuccess) {
				return new GetByIdFlatTaskQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}

			return new GetByIdFlatTaskQueryResponse {
				Success = true,
				Task = mapper.Map<FlatTaskDto>(result.Value)
			};
		}
	}
}
