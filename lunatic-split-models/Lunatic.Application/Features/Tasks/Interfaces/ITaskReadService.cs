﻿using Lunatic.Application.Contracts.Interfaces;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Domain.Utils;

namespace Lunatic.Application.Features.Tasks.Interfaces {
	public interface ITaskReadService : IGenericReadService<TaskReadModel> {
		Task<Result<FlatTaskReadModel>> GetFlatTaskByIdAsync(Guid taskId);
		Task<Result<TaskDescriptionReadModel>> GetTaskDescriptionByIdAsync(Guid taskId);

	}
}
