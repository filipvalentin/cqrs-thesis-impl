using Lunatic.Domain.MLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunatic.Application.Features.Tasks.Interfaces {
	public interface IMLDataProvider {
		Task<int> GetTaskPrediction(DaysToCompleteTaskEntry payload);
	}
}
