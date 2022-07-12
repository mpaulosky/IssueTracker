using Microsoft.Extensions.Options;

namespace IssueTracker.Library.DataAccess;

public class MongoDbContext : IMongoDbContext
{
	public MongoDbContext(IOptions<DatabaseSettings> configuration)
	{
		DbName = configuration.Value.DatabaseName;
		Client = new MongoClient(configuration.Value.ConnectionString);
		Database = Client.GetDatabase(DbName);
	}

	public IMongoDatabase Database { get; }
	public IMongoClient Client { get; }
	public string DbName { get; }

	public IMongoCollection<T> GetCollection<T>(string name)
	{
		return Database.GetCollection<T>(name);
	}
}