using Lunatic.Application.Contracts.Interfaces;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Domain.Utils;

namespace Lunatic.Application.Features.Tasks
{
    public interface ITaskReadService : IGenericReadService<TaskReadModel> {
		Task<Result<CompoundTaskReadModel>> GetCompoundTaskByIdAsync(Guid taskId);
	}
}
