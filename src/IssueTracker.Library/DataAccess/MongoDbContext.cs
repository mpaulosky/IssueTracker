using Microsoft.Extensions.Options;

namespace IssueTracker.Library.DataAccess;

public class MongoDbContext : IMongoDbContext
{
	public IMongoDatabase Database { get; private set; }
	public IMongoClient Client { get; private set; }
	public string DbName { get; private set; }
	
	public MongoDbContext(IOptions<DatabaseSettings> configuration)
	{
		DbName = configuration.Value.DatabaseName;
		Client = new MongoClient(configuration.Value.ConnectionString);
		Database = Client.GetDatabase(DbName);
	}
	
	public IMongoCollection<T> GetCollection<T>(string name)
	{
		return Database.GetCollection<T>(name);
	}
}