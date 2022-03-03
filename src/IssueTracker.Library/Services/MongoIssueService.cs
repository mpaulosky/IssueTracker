using Microsoft.Extensions.Caching.Memory;

namespace IssueTrackerLibrary.Services;

public class MongoIssueService : IIssueService
{
	private readonly IMongoDbContext _db;
	private readonly IUserService _userData;
	private readonly IMemoryCache _cache;
	private readonly IMongoCollection<Issue> _issues;
	private const string _cacheName = "IssueData";

	public MongoIssueService(IMongoDbContext db, IUserService userData, IMemoryCache cache)
	{
		_db = db;
		_userData = userData;
		_cache = cache;
		_issues = db.GetCollection<Issue>(CollectionNames.GetCollectionName(nameof(Issue)));
	}

	public async Task<List<Issue>> GetAllIssues()
	{
		var output = _cache.Get<List<Issue>>(_cacheName);
		
		if (output is null)
		{
			var results = await _issues.FindAsync(s => s.Archived == false);
			output = results.ToList();

			_cache.Set(_cacheName, output, TimeSpan.FromMinutes(1));
		}

		return output;
	}

	public async Task<List<Issue>> GetUsersIssues(string userId)
	{
		var output = _cache.Get<List<Issue>>(userId);
		
		if (output is null)
		{
			var results = await _issues.FindAsync(s => s.Author.Id == userId);
			output = results.ToList();

			_cache.Set(userId, output, TimeSpan.FromMinutes(1));
		}

		return output;
	}

	public async Task<List<Issue>> GetAllApprovedIssues()
	{
		List<Issue> output = await GetAllIssues();
		return output.ToList();
	}

	public async Task<Issue> GetIssue(string id)
	{
		var results = await _issues.FindAsync(s => s.Id == id);
		return results.FirstOrDefault();
	}

	public async Task<List<Issue>> GetAllIssuesWaitingForApproval()
	{
		List<Issue> output = await GetAllIssues();
		return output.ToList();
	}

	public async Task UpdateIssue(Issue suggestion)
	{
		await _issues.ReplaceOneAsync(s => s.Id == suggestion.Id, suggestion);
		_cache.Remove(_cacheName);
	}

	public async Task UpvoteIssue(string suggestionId, string userId)
	{
		var client = _db.Client;

		using var session = await client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			// IMongoDatabase? db = client.GetDatabase(_db.DbName);
			// IMongoCollection<Issue>? suggestionsInTransaction = db.GetCollection<Issue>(_db.IssueCollectionName);
			// Issue? suggestion = (await suggestionsInTransaction.FindAsync(s => s.Id == suggestionId)).First();
			//
			// await suggestionsInTransaction.ReplaceOneAsync(session, s => s.Id == suggestionId, suggestion);
			//
			// IMongoCollection<User>? usersInTransaction = db.GetCollection<User>(_db.UserCollectionName);
			// User user = await _userData.GetUser(userId);
			//
			// await usersInTransaction.ReplaceOneAsync(session, u => u.Id == userId, user);
			//
			// await session.CommitTransactionAsync();
			//
			// _cache.Remove(_cacheName);
		}
		catch (Exception ex)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}

	public async Task CreateIssue(Issue suggestion)
	{
		MongoClient client = _db.Client;

		using var session = await client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			// IMongoDatabase? db = client.GetDatabase(_db.DbName);
			// IMongoCollection<Issue>? suggestionsInTransaction = db.GetCollection<Issue>(_db.IssueCollectionName);
			// await suggestionsInTransaction.InsertOneAsync(session, suggestion);
			//
			// IMongoCollection<User>? usersInTransaction = db.GetCollection<User>(_db.UserCollectionName);
			// User user = await _userData.GetUser(suggestion.Author.Id);
			// user.AuthoredIssues.Add(new BasicIssueModel(suggestion));
			// await usersInTransaction.ReplaceOneAsync(session, u => u.Id == user.Id, user);
			//
			// await session.CommitTransactionAsync();
		}
		catch (Exception ex)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}
}