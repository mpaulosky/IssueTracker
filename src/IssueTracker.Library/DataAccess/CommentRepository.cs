//-----------------------------------------------------------------------
// <copyright file="CommentRepository.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.Library.DataAccess;

/// <summary>
///   CommentRepository class
/// </summary>
public class CommentRepository : ICommentRepository
{
	private readonly IMongoDbContext _context;
	private readonly IMongoCollection<CommentModel> _commentCollection;
	private readonly IMongoCollection<UserModel> _userCollection;

	/// <summary>
	///   CommentRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	public CommentRepository(IMongoDbContext context)
	{
		_context = Guard.Against.Null(context, nameof(context));

		string commentCollectionName;
		
		commentCollectionName = Guard.Against.NullOrWhiteSpace(GetCollectionName(nameof(CommentModel)), nameof(commentCollectionName));

		_commentCollection = _context.GetCollection<CommentModel>(commentCollectionName);

		string userCollectionName;
		
		userCollectionName = Guard.Against.NullOrWhiteSpace(GetCollectionName(nameof(UserModel)), nameof(userCollectionName));
		
		_userCollection = _context.GetCollection<UserModel>(userCollectionName);
	}

	/// <summary>
	///   CreateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateComment(CommentModel comment)
	{
		using var session = await _context.Client.StartSessionAsync().ConfigureAwait(true);

		session.StartTransaction();

		try
		{
			var commentsInTransaction = _commentCollection;

			await commentsInTransaction.InsertOneAsync(comment).ConfigureAwait(true);

			var usersInTransaction = _userCollection;

			var user = (await _userCollection.FindAsync(u => u.Id == comment.Author.Id).ConfigureAwait(true)).First();

			user.AuthoredComments.Add(new BasicCommentModel(comment));

			await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user).ConfigureAwait(true);

			await session.CommitTransactionAsync().ConfigureAwait(true);
		}
		catch (Exception)
		{
			await session.AbortTransactionAsync().ConfigureAwait(true);
			throw;
		}
	}

	/// <summary>
	///   GetComment method
	/// </summary>
	/// <param name="id">string</param>
	/// <returns>Task of CommentModel</returns>
	public async Task<CommentModel> GetComment(string id)
	{
		var objectId = new ObjectId(id);

		FilterDefinition<CommentModel> filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		var result = await _commentCollection.FindAsync(filter).ConfigureAwait(true);

		return result.FirstOrDefault();
	}

	/// <summary>
	///   GetComments method
	/// </summary>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetComments()
	{
		var all = await _commentCollection.FindAsync(Builders<CommentModel>.Filter.Empty).ConfigureAwait(true);

		return await all.ToListAsync().ConfigureAwait(true);
	}

	/// <summary>
	///   GetIssueComments method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetIssuesComments(string issueId)
	{
		var objectId = new ObjectId(issueId);

		var results = await _commentCollection.FindAsync(s => s.Issue.Id == objectId.ToString()).ConfigureAwait(true);

		return await results.ToListAsync().ConfigureAwait(true);
	}

	/// <summary>
	///   GetUserComments method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetUsersComments(string userId)
	{
		var objectId = new ObjectId(userId);

		var results = await _commentCollection.FindAsync(s => s.Author.Id == objectId.ToString()).ConfigureAwait(true);

		return await results.ToListAsync().ConfigureAwait(true);
	}

	/// <summary>
	///   UpdateComment method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="comment">CommentModel</param>
	public async Task UpdateComment(string id, CommentModel comment)
	{
		var objectId = new ObjectId(id);

		await _commentCollection.ReplaceOneAsync(Builders<CommentModel>.Filter.Eq("_id", objectId), comment)
			.ConfigureAwait(true);
	}

	/// <summary>
	///   UpvoteComment method
	/// </summary>
	/// <param name="commentId">string</param>
	/// <param name="userId">string</param>
	public async Task UpVoteComment(string commentId, string userId)
	{
		using var session = await _context.Client.StartSessionAsync().ConfigureAwait(true);

		session.StartTransaction();

		try
		{
			var commentsInTransaction = _commentCollection;

			var objectId = new ObjectId(commentId);

			FilterDefinition<CommentModel> filterComment = Builders<CommentModel>.Filter.Eq("_id", objectId);

			var comment = (await commentsInTransaction.FindAsync(filterComment).ConfigureAwait(true)).FirstOrDefault();

			bool isUpvote = comment.UserVotes.Add(userId);

			if (isUpvote == false)
			{
				comment.UserVotes.Remove(userId);
			}

			await commentsInTransaction.ReplaceOneAsync(session, s => s.Id == commentId, comment).ConfigureAwait(true);

			await session.CommitTransactionAsync().ConfigureAwait(true);
		}
		catch (Exception)
		{
			await session.AbortTransactionAsync().ConfigureAwait(true);
			throw;
		}
	}
}