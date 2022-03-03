using IssueTrackerLibrary.Contracts;
using IssueTrackerLibrary.Helpers;

using Microsoft.Extensions.Options;

namespace IssueTrackerLibrary.DataAccess;

public class MongoDbContext : IMongoDbContext
{
	private readonly IMongoDatabase _database;
	public MongoClient Client { get; private set; }
	public string DbName { get; private set; }
	
	public MongoDbContext(IOptions<IssueTrackerDatabaseSettings> configuration)
	{
		DbName = configuration.Value.DatabaseName;
		Client = new MongoClient(configuration.Value.ConnectionString);
		_database = Client.GetDatabase(DbName);
	}
	
	public IMongoCollection<T> GetCollection<T>(string name)
	{
		return _database.GetCollection<T>(name);
	}
}