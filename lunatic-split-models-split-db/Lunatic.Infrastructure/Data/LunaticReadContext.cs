using Lunatic.Application.Contracts;
using MongoDB.Driver;

namespace Lunatic.Infrastructure.Data {
	public class LunaticReadContext : ILunaticReadContext {
		private readonly IMongoDatabase _database;

		public LunaticReadContext(string connectionString, string databaseName) {
			var client = new MongoClient(connectionString);
			_database = client.GetDatabase(databaseName);
		}

		public IMongoCollection<T> GetCollection<T>() where T : class {
			return _database.GetCollection<T>(typeof(T).Name);
		}
	}
}

