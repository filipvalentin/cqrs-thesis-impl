using MongoDB.Driver;

namespace Lunatic.Application.Contracts {
	public interface ILunaticReadContext {
		IMongoCollection<T> GetCollection<T>() where T : class;
	}
}
