using Microsoft.Extensions.Configuration;

namespace IssueTrackerLibrary.DataAccess;

public class DbConnection : IDbConnection
{
	private readonly IConfiguration _config;
	private readonly IMongoDatabase _db;
	private string _connectionId = "MongoDB";

	public string DbName { get; private set; }
	public string StatusCollectionName { get; private set; } = "statuses";
	public string UserCollectionName { get; private set; } = "users";
	public string IssueCollectionName { get; private set; } = "issues";
	public string CommentCollectionName { get; private set; } = "comments";
	public MongoClient Client { get; private set; }

	public IMongoCollection<StatusModel> StatusCollection { get; private set; }
	public IMongoCollection<UserModel> UserCollection { get; private set; }
	public IMongoCollection<IssueModel> IssueCollection { get; private set; }
	public IMongoCollection<CommentModel> CommentCollection { get; private set; }


	public DbConnection(IConfiguration config)
	{
		_config = config;
		Client = new MongoClient(_config.GetConnectionString(_connectionId));
		DbName = _config["DatabaseName"];
		_db = Client.GetDatabase(DbName);

		StatusCollection = _db.GetCollection<StatusModel>(StatusCollectionName);
		UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
		IssueCollection = _db.GetCollection<IssueModel>(IssueCollectionName);
		CommentCollection = _db.GetCollection<CommentModel>(CommentCollectionName);
	}
}