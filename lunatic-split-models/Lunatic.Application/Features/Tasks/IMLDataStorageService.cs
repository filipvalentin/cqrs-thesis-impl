using Lunatic.Domain.MLModel;

namespace Lunatic.Application.Features.Tasks {
	public interface IMLDataStorageService {
		public Task AddEntryAsync(DaysToCompleteTaskEntry entry);
	}
}
