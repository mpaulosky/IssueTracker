using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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

public class MongoDbContext : IMongoDbContext
{
	private readonly IMongoDatabase _db;
	private readonly MongoClient _client;
	public IClientSessionHandle Session { get; set; }

	private string StatusCollectionName { get; set; } = "statuses";
	private string UserCollectionName { get; set; } = "users";
	private string IssueCollectionName { get; set; } = "issues";
	private string CommentCollectionName { get; set; } = "comments";
	
	public MongoClient Client { get; private set; }

	public IMongoCollection<StatusModel> StatusCollection { get; private set; }
	public IMongoCollection<UserModel> UserCollection { get; private set; }
	public IMongoCollection<IssueModel> IssueCollection { get; private set; }
	public IMongoCollection<CommentModel> CommentCollection { get; private set; }

	public MongoDbContext(IOptions<IssueTrackerDatabaseSettings> configuration)
	{
		_client = new MongoClient(configuration.Value.ConnectionString);
		_db = _client.GetDatabase(configuration.Value.DatabaseName);
		
		StatusCollection = _db.GetCollection<StatusModel>(StatusCollectionName);
		UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
		IssueCollection = _db.GetCollection<IssueModel>(IssueCollectionName);
		CommentCollection = _db.GetCollection<CommentModel>(CommentCollectionName);
	}
	
	public IMongoCollection<T> GetCollection<T>(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
		}

		return _db.GetCollection<T>(name);
	}
}