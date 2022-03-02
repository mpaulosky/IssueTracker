using IssueTrackerLibrary.Contracts;

using Microsoft.Extensions.Options;

namespace IssueTrackerLibrary.DataAccess;

public class DbConnection : IDbConnection
{
	private readonly IMongoDatabase _database;
	public MongoClient Client { get; }
	public string DbName { get; }
	
	public string StatusCollectionName { get; } = "statuses";
	public string UserCollectionName { get; } = "users";
	public string IssueCollectionName { get; } = "issues";
	public string CommentCollectionName { get; } = "comments";

	public IMongoCollection<Status> StatusCollection { get; }
	public IMongoCollection<User> UserCollection { get; }
	public IMongoCollection<Issue> IssueCollection { get; }
	public IMongoCollection<Comment> CommentCollection { get; }


	public DbConnection(IOptions<IssueTrackerDatabaseSettings> configuration)
	{
		DbName = configuration.Value.DatabaseName;
		Client = new MongoClient(configuration.Value.ConnectionString);
		_database = Client.GetDatabase(DbName);

		StatusCollection = _database.GetCollection<Status>(StatusCollectionName);
		UserCollection = _database.GetCollection<User>(UserCollectionName);
		IssueCollection = _database.GetCollection<Issue>(IssueCollectionName);
		CommentCollection = _database.GetCollection<Comment>(CommentCollectionName);
	}
}