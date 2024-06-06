using Lunatic.Infrastructure.Data;
using Lunatic.Domain.MLModel;
using Lunatic.Application.Features.Tasks.Interfaces;

namespace Lunatic.Infrastructure.Services {
	public class MLDataStorageService : IMLDataStorageService {

		public LunaticMLContext context;

		public MLDataStorageService(LunaticMLContext context) {
			this.context = context;
		}

		public async Task AddEntryAsync(DaysToCompleteTaskEntry entry) {
			var x = await context.DaysToCompleteTaskEntries.AddAsync(entry);
			await context.SaveChangesAsync();
		}
	}
}
