using IssueTrackerLibrary.Contracts;

using Microsoft.Extensions.Options;

namespace IssueTrackerLibrary.DataAccess;

public class DbConnection : IDbConnection
{
	private readonly IMongoDatabase _database;
	public MongoClient Client { get; private set; }
	public string DbName { get; private set; }
	
	public string StatusCollectionName { get; private set; } = "statuses";
	public string UserCollectionName { get; private set; } = "users";
	public string IssueCollectionName { get; private set; } = "issues";
	public string CommentCollectionName { get; private set; } = "comments";

	public IMongoCollection<Status> StatusCollection { get; private set; }
	public IMongoCollection<User> UserCollection { get; private set; }
	public IMongoCollection<Issue> IssueCollection { get; private set; }
	public IMongoCollection<Comment> CommentCollection { get; private set; }


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