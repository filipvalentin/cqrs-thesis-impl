using Lunatic.Domain.MLModel;


namespace Lunatic.Application.Persistence {
	public interface IMLRepository : IAsyncRepository<DaysToCompleteTaskEntry> {
    }
}

