using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.ReadSide.Task;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTaskDescription {
	public class GetByIdTaskDescriptionQueryHandler : IRequestHandler<GetByIdTaskDescriptionQuery, GetByIdTaskDescriptionQueryResponse> {
		private readonly ITaskDescriptionReadSideRepository taskDescriptionReadRepository;
		private readonly IMapper mapper;

		public GetByIdTaskDescriptionQueryHandler(ITaskDescriptionReadSideRepository taskDescriptionReadRepository, IMapper mapper) {
			this.taskDescriptionReadRepository = taskDescriptionReadRepository;
			this.mapper = mapper;
		}

		public async Task<GetByIdTaskDescriptionQueryResponse> Handle(GetByIdTaskDescriptionQuery request, CancellationToken cancellationToken) {
			var result = await taskDescriptionReadRepository.FindByIdAsync(request.TaskId);

			if (!result.IsSuccess) {
				return new GetByIdTaskDescriptionQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}

			return new GetByIdTaskDescriptionQueryResponse {
				Success = true,
				Task = mapper.Map<TaskDescriptionDto>(result.Value)
			};
		}
	}
}
