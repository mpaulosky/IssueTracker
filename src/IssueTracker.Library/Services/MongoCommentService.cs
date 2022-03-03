using Microsoft.Extensions.Caching.Memory;

using static IssueTrackerLibrary.Helpers.CollectionNames;

namespace IssueTrackerLibrary.Services;

public class MongoCommentService : ICommentService
{
	private readonly IMongoDbContext _db;
	private readonly IUserService _userData;
	private readonly IMemoryCache _cache;
	private readonly IMongoCollection<Comment> _comments;
	private const string _cacheName = "CommentData";

	public MongoCommentService(IMongoDbContext db, IUserService userData, IMemoryCache cache)
	{
		_db = db;
		_userData = userData;
		_cache = cache;
		_comments = db.GetCollection<Comment>(GetCollectionName(nameof(Comment)));
	}

	public async Task<List<Comment>> GetAllComments()
	{
		var output = _cache.Get<List<Comment>>(_cacheName);
		if (output is null)
		{
			var results = await _comments.FindAsync(s => s.Archived == false);
			output = results.ToList();

			_cache.Set(_cacheName, output, TimeSpan.FromMinutes(1));
		}

		return output;
	}

	public async Task<List<Comment>> GetUsersComments(string userId)
	{
		var output = _cache.Get<List<Comment>>(userId);
		if (output is null)
		{
			IAsyncCursor<Comment> results = await _comments.FindAsync(s => s.Author.Id == userId);
			output = results.ToList();

			_cache.Set(userId, output, TimeSpan.FromMinutes(1));
		}

		return output;
	}

	public async Task<List<Comment>> GetAllApprovedComments()
	{
		var output = await GetAllComments();
		return output.ToList();
	}

	public async Task<Comment> GetComment(string id)
	{
		var results = await _comments.FindAsync(s => s.Id == id);
		return results.FirstOrDefault();
	}

	public async Task<List<Comment>> GetAllCommentsWaitingForApproval()
	{
		var output = await GetAllComments();
		return output.ToList();
	}

	public async Task UpdateComment(Comment suggestion)
	{
		await _comments.ReplaceOneAsync(s => s.Id == suggestion.Id, suggestion);
		_cache.Remove(_cacheName);
	}

	public async Task UpvoteComment(string commentId, string userId)
	{
		MongoClient client = _db.Client;

		using IClientSessionHandle session = await client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			// IMongoDatabase? db = client.GetDatabase(_db.DbName);
			// IMongoCollection<Comment>? suggestionsInTransaction = db.GetCollection<Comment>(_db.CommentCollectionName);
			// Comment? comment = (await suggestionsInTransaction.FindAsync(s => s.Id == commentId)).First();

			// bool isUpvote = comment.UserVotes.Add(userId);
			// if (isUpvote == false)
			// {
			// 	comment.UserVotes.Remove(userId);
			// }
			//
			// await suggestionsInTransaction.ReplaceOneAsync(session, s => s.Id == commentId, comment);
			//
			// IMongoCollection<User>? usersInTransaction = db.GetCollection<User>(_db.UserCollectionName);
			User user = await _userData.GetUser(userId);

			// if (isUpvote)
			// {
			// 	user.VotedOnComments.Add(new BasicCommentModel(comment));
			// }
			// else
			// {
			// 	BasicCommentModel commentToRemove = user.VotedOnComments.First(s => s.Id == commentId);
			// 	user.VotedOnComments.Remove(commentToRemove);
			// }
			// await usersInTransaction.ReplaceOneAsync(session, u => u.Id == userId, user);

			await session.CommitTransactionAsync();

			_cache.Remove(_cacheName);
		}
		catch (Exception ex)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}

	public async Task CreateComment(Comment comment)
	{
		var client = _db.Client;

		using var session = await client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			// IMongoDatabase? db = client.GetDatabase(_db.DbName);
			// IMongoCollection<Comment>? suggestionsInTransaction = db.GetCollection<Comment>(_db.CommentCollectionName);
			// await suggestionsInTransaction.InsertOneAsync(session, comment);
			//
			// IMongoCollection<User>? usersInTransaction = db.GetCollection<User>(_db.UserCollectionName);
			// User user = await _userData.GetUser(comment.Author.Id);
			// user.AuthoredComments.Add(new BasicCommentModel(comment));
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