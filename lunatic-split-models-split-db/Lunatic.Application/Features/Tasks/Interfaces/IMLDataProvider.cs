using Lunatic.Domain.MLModel;

namespace Lunatic.Application.Features.Tasks.Interfaces {
	public interface IMLDataProvider {
		Task<int> GetTaskPrediction(DaysToCompleteTaskEntry payload);
	}
}
