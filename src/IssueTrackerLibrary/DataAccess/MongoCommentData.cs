using Microsoft.Extensions.Caching.Memory;

namespace IssueTrackerLibrary.DataAccess;

public class MongoCommentData : ICommentData
{
	private readonly IDbConnection _db;
	private readonly IUserData _userData;
	private readonly IMemoryCache _cache;
	private readonly IMongoCollection<CommentModel> _suggestions;
	private const string _cacheName = "CommentData";

	public MongoCommentData(IDbConnection db, IUserData userData, IMemoryCache cache)
	{
		_db = db;
		_userData = userData;
		_cache = cache;
		_suggestions = db.CommentCollection;
	}

	public async Task<List<CommentModel>> GetAllComments()
	{
		List<CommentModel>? output = _cache.Get<List<CommentModel>>(_cacheName);
		if (output is null)
		{
			IAsyncCursor<CommentModel>? results = await _suggestions.FindAsync(s => s.Archived == false);
			output = results.ToList();

			_cache.Set(_cacheName, output, TimeSpan.FromMinutes(1));
		}

		return output;
	}

	public async Task<List<CommentModel>> GetUsersComments(string userId)
	{
		List<CommentModel>? output = _cache.Get<List<CommentModel>>(userId);
		if (output is null)
		{
			IAsyncCursor<CommentModel>? results = await _suggestions.FindAsync(s => s.Author.Id == userId);
			output = results.ToList();

			_cache.Set(userId, output, TimeSpan.FromMinutes(1));
		}

		return output;
	}

	public async Task<List<CommentModel>> GetAllApprovedComments()
	{
		List<CommentModel> output = await GetAllComments();
		return output.ToList();
	}

	public async Task<CommentModel> GetComment(string id)
	{
		IAsyncCursor<CommentModel>? results = await _suggestions.FindAsync(s => s.Id == id);
		return results.FirstOrDefault();
	}

	public async Task<List<CommentModel>> GetAllCommentsWaitingForApproval()
	{
		List<CommentModel> output = await GetAllComments();
		return output.ToList();
	}

	public async Task UpdateComment(CommentModel suggestion)
	{
		await _suggestions.ReplaceOneAsync(s => s.Id == suggestion.Id, suggestion);
		_cache.Remove(_cacheName);
	}

	public async Task UpvoteComment(string commentId, string userId)
	{
		MongoClient client = _db.Client;

		using IClientSessionHandle? session = await client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			IMongoDatabase? db = client.GetDatabase(_db.DbName);
			IMongoCollection<CommentModel>? suggestionsInTransaction = db.GetCollection<CommentModel>(_db.CommentCollectionName);
			CommentModel? comment = (await suggestionsInTransaction.FindAsync(s => s.Id == commentId)).First();

			bool isUpvote = comment.UserVotes.Add(userId);
			if (isUpvote == false)
			{
				comment.UserVotes.Remove(userId);
			}

			await suggestionsInTransaction.ReplaceOneAsync(session, s => s.Id == commentId, comment);

			IMongoCollection<UserModel>? usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
			UserModel user = await _userData.GetUser(userId);

			if (isUpvote)
			{
				user.VotedOnComments.Add(new BasicCommentModel(comment));
			}
			else
			{
				BasicCommentModel commentToRemove = user.VotedOnComments.First(s => s.Id == commentId);
				user.VotedOnComments.Remove(commentToRemove);
			}
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

	public async Task CreateComment(CommentModel comment)
	{
		MongoClient client = _db.Client;

		using IClientSessionHandle? session = await client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			IMongoDatabase? db = client.GetDatabase(_db.DbName);
			IMongoCollection<CommentModel>? suggestionsInTransaction = db.GetCollection<CommentModel>(_db.CommentCollectionName);
			await suggestionsInTransaction.InsertOneAsync(session, comment);

			IMongoCollection<UserModel>? usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
			UserModel user = await _userData.GetUser(comment.Author.Id);
			user.AuthoredComments.Add(new BasicCommentModel(comment));
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