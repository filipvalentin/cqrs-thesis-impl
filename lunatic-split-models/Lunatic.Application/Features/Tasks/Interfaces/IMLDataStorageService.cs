using Lunatic.Domain.MLModel;

namespace Lunatic.Application.Features.Tasks.Interfaces {
	public interface IMLDataStorageService {
		public Task AddEntryAsync(DaysToCompleteTaskEntry entry);
	}
}
