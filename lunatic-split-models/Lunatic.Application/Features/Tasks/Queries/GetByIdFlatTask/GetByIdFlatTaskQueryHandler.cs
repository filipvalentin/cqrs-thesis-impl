using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Features.Tasks.Payload;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdCompositeTask {
	public class GetByIdFlatTaskQueryHandler : IRequestHandler<GetByIdFlatTaskQuery, GetByIdFlatTaskQueryResponse> {
		private readonly ITaskReadService taskReadService;

		public GetByIdFlatTaskQueryHandler(ITaskReadService taskReadService) {
			this.taskReadService = taskReadService;
		}


		public async Task<GetByIdFlatTaskQueryResponse> Handle(GetByIdFlatTaskQuery request, CancellationToken cancellationToken) {

			var result = await taskReadService.GetFlatTaskByIdAsync(request.TaskId);

			if (!result.IsSuccess) {
				return new GetByIdFlatTaskQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}
			var taskReadModel = result.Value;

			return new GetByIdFlatTaskQueryResponse {
				Success = true,
				Task = new CompositeTaskDto {
					CreatedByUserId = taskReadModel.CreatedByUserId,
					TaskId = taskReadModel.Id,
					ProjectId = taskReadModel.ProjectId,
					Section = taskReadModel.TaskSectionCard,
					Title = taskReadModel.Title,
					Description = taskReadModel.Description,
					Priority = taskReadModel.Priority,
					Status = taskReadModel.Status,
					Tags = taskReadModel.Tags,
					Comments = taskReadModel.Comments,
					AssigneeIds = taskReadModel.AssigneeIds,
					PlannedStartDate = taskReadModel.PlannedStartDate,
					PlannedEndDate = taskReadModel.PlannedEndDate,
					StartedDate = taskReadModel.StartedDate,
					EndedDate = taskReadModel.EndedDate,
				}
			};
		}
	}
}
