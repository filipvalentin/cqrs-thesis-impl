using Lunatic.Application.Features.Tasks.Payload;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdCompoundTask {
	public class GetByIdCompoundTaskQueryHandler : IRequestHandler<GetByIdCompoundTaskQuery, GetByIdCompoundTaskQueryResponse> {
		private readonly ITaskReadService taskReadService;

		public GetByIdCompoundTaskQueryHandler(ITaskReadService taskReadService) {
			this.taskReadService = taskReadService;
		}


		public async Task<GetByIdCompoundTaskQueryResponse> Handle(GetByIdCompoundTaskQuery request, CancellationToken cancellationToken) {

			var result = await taskReadService.GetCompoundTaskByIdAsync(request.TaskId);

			if (!result.IsSuccess) {
				return new GetByIdCompoundTaskQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}
			var taskReadModel = result.Value;

			return new GetByIdCompoundTaskQueryResponse {
				Success = true,
				Task = new CompoundTaskDto {
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
