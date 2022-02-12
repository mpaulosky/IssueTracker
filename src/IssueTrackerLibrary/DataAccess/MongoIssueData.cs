using Microsoft.Extensions.Caching.Memory;

namespace IssueTrackerLibrary.DataAccess;

public class MongoIssueData : IIssueData
{
	private readonly IDbConnection _db;
	private readonly IUserData _userData;
	private readonly IMemoryCache _cache;
	private readonly IMongoCollection<IssueModel> _issues;
	private const string _cacheName = "IssueData";

	public MongoIssueData(IDbConnection db, IUserData userData, IMemoryCache cache)
	{
		_db = db;
		_userData = userData;
		_cache = cache;
		_issues = db.IssueCollection;
	}

	public async Task<List<IssueModel>> GetAllSuggestions()
	{
		List<IssueModel>? output = _cache.Get<List<IssueModel>>(_cacheName);
		if (output is null)
		{
			IAsyncCursor<IssueModel>? results = await _issues.FindAsync(s => s.Archived == false);
			output = results.ToList();

			_cache.Set(_cacheName, output, TimeSpan.FromMinutes(1));
		}

		return output;
	}

	public async Task<List<IssueModel>> GetUsersSuggestions(string userId)
	{
		List<IssueModel>? output = _cache.Get<List<IssueModel>>(userId);
		if (output is null)
		{
			IAsyncCursor<IssueModel>? results = await _issues.FindAsync(s => s.Author.Id == userId);
			output = results.ToList();

			_cache.Set(userId, output, TimeSpan.FromMinutes(1));
		}

		return output;
	}

	public async Task<List<IssueModel>> GetAllApprovedSuggestions()
	{
		List<IssueModel> output = await GetAllSuggestions();
		return output.ToList();
	}

	public async Task<IssueModel> GetSuggestion(string id)
	{
		IAsyncCursor<IssueModel>? results = await _issues.FindAsync(s => s.Id == id);
		return results.FirstOrDefault();
	}

	public async Task<List<IssueModel>> GetAllSuggestionsWaitingForApproval()
	{
		List<IssueModel> output = await GetAllSuggestions();
		return output.ToList();
	}

	public async Task UpdateSuggestion(IssueModel suggestion)
	{
		await _issues.ReplaceOneAsync(s => s.Id == suggestion.Id, suggestion);
		_cache.Remove(_cacheName);
	}

	public async Task UpvoteSuggestion(string suggestionId, string userId)
	{
		MongoClient client = _db.Client;

		using IClientSessionHandle? session = await client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			IMongoDatabase? db = client.GetDatabase(_db.DbName);
			IMongoCollection<IssueModel>? suggestionsInTransaction = db.GetCollection<IssueModel>(_db.IssueCollectionName);
			IssueModel? suggestion = (await suggestionsInTransaction.FindAsync(s => s.Id == suggestionId)).First();

			await suggestionsInTransaction.ReplaceOneAsync(session, s => s.Id == suggestionId, suggestion);

			IMongoCollection<UserModel>? usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
			UserModel user = await _userData.GetUser(userId);

			await usersInTransaction.ReplaceOneAsync(session, u => u.Id == userId, user);

			await session.CommitTransactionAsync();

			_cache.Remove(_cacheName);
		}
		catch (Exception ex)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}

	public async Task CreateSuggestion(IssueModel suggestion)
	{
		MongoClient client = _db.Client;

		using var session = await client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			IMongoDatabase? db = client.GetDatabase(_db.DbName);
			IMongoCollection<IssueModel>? suggestionsInTransaction = db.GetCollection<IssueModel>(_db.IssueCollectionName);
			await suggestionsInTransaction.InsertOneAsync(session, suggestion);

			IMongoCollection<UserModel>? usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
			UserModel user = await _userData.GetUser(suggestion.Author.Id);
			user.AuthoredIssues.Add(new BasicIssueModel(suggestion));
			await usersInTransaction.ReplaceOneAsync(session, u => u.Id == user.Id, user);

			await session.CommitTransactionAsync();
		}
		catch (Exception ex)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}
}