using IssueTrackerLibrary.Contracts;

using Microsoft.Extensions.Options;

namespace IssueTrackerLibrary.DataAccess;

public class MongoDbContext : IMongoDbContext
{
	private readonly IMongoDatabase _database;
	public MongoClient Client { get; private set; }
	public string DbName { get; private set; }


	// public string StatusCollectionName { get; set; } = "statuses";
	// public string UserCollectionName { get; set; } = "users";
	// public string IssueCollectionName { get; set; } = "issues";
	// public string CommentCollectionName { get; set; } = "comments";

	// public IMongoCollection<Status> StatusCollection { get; set; }
	// public IMongoCollection<User> UserCollection { get; set; }
	// public IMongoCollection<Issue> IssueCollection { get; set; }
	// public IMongoCollection<Comment> CommentCollection { get; set; }

	public MongoDbContext(IOptions<IssueTrackerDatabaseSettings> configuration)
	{
		DbName = configuration.Value.DatabaseName;
		Client = new MongoClient(configuration.Value.ConnectionString);
		_database = Client.GetDatabase(DbName);
		
		// StatusCollection = _database.GetCollection<Status>(StatusCollectionName);
		// UserCollection = _database.GetCollection<User>(UserCollectionName);
		// IssueCollection = _database.GetCollection<Issue>(IssueCollectionName);
		// CommentCollection = _database.GetCollection<Comment>(CommentCollectionName);
	}
	
	public IMongoCollection<T> GetCollection<T>(string name)
	{
		return _database.GetCollection<T>(name);
	}
}