//-----------------------------------------------------------------------
// <copyright file="CommentRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.Library.Helpers.BogusFakes;

namespace IssueTracker.Library.DataAccess;

/// <summary>
///		CommentRepository class
/// </summary>
public class CommentRepository : ICommentRepository
{

	private readonly IMongoCollection<CommentModel> _commentCollection;
	private readonly IMongoDbContextFactory _context;
	private readonly IMongoCollection<UserModel> _userCollection;

	/// <summary>
	///		CommentRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CommentRepository(IMongoDbContextFactory context)
	{

		_context = Guard.Against.Null(context, nameof(context));

		var commentCollectionName = GetCollectionName(nameof(CommentModel));

		_commentCollection = context.GetCollection<CommentModel>(commentCollectionName);

		var userCollectionName = GetCollectionName(nameof(UserModel));

		_userCollection = context.GetCollection<UserModel>(userCollectionName);

	}

	/// <summary>
	///		CreateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="Exception"></exception>
	public async Task CreateComment(CommentModel comment)
	{

		using var session = await _context.Client.StartSessionAsync().ConfigureAwait(true);

		session.StartTransaction();

		try
		{

			var commentsInTransaction = _commentCollection!;

			await commentsInTransaction.InsertOneAsync(comment);

			var usersInTransaction = _userCollection;

			var user = (await _userCollection!.FindAsync(u => u.Id == comment.Author.Id).ConfigureAwait(true))
				.First();

			user.AuthoredComments.Add(new BasicCommentModel(comment));

			var replaceOneResult =
				await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

			await session.CommitTransactionAsync();

		}
		catch (Exception)
		{

			await session.AbortTransactionAsync();
			throw;

		}
	}

	/// <summary>
	///		GetComment method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of CommentModel</returns>
	public async Task<CommentModel> GetComment(string itemId)
	{

		var objectId = new ObjectId(itemId);

		var filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		var result = (await _commentCollection.FindAsync(filter)).FirstOrDefault();

		return result;

	}

	/// <summary>
	///		GetComments method
	/// </summary>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetComments()
	{

		var filter = Builders<CommentModel>.Filter.Empty;

		var results = (await _commentCollection.FindAsync(filter)).ToList();

		return results;

	}

	/// <summary>
	///		GetIssueComments method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetIssuesComments(string issueId)
	{

		var results = (await _commentCollection.FindAsync(s => s.Issue!.Id! == issueId)).ToList();

		return results;

	}

	/// <summary>
	///		GetUserComments method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetUsersComments(string userId)
	{

		var results = (await _commentCollection.FindAsync(s => s.Author.Id == userId)).ToList();

		return results;

	}

	/// <summary>
	///		UpdateComment method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="comment">CommentModel</param>
	public async Task UpdateComment(string itemId, CommentModel comment)
	{

		var objectId = new ObjectId(itemId);

		var filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		await _commentCollection.ReplaceOneAsync(filter, comment);

	}

	/// <summary>
	///		UpvoteComment method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="userId">string</param>
	/// <exception cref="Exception"></exception>
	public async Task UpVoteComment(string itemId, string userId)
	{

		using var session = await _context.Client.StartSessionAsync().ConfigureAwait(true);

		session.StartTransaction();

		try
		{

			var commentsInTransaction = _commentCollection;

			var objectId = new ObjectId(itemId);

			var filterComment = Builders<CommentModel>.Filter.Eq("_id", objectId);

			var comment = (await commentsInTransaction.FindAsync(filterComment)).FirstOrDefault();

			var isUpvote = comment.UserVotes.Add(userId);

			if (isUpvote == false)
			{

				comment.UserVotes.Remove(userId);

			}

			await commentsInTransaction.ReplaceOneAsync(session, s => s.Id == itemId, comment);

			await session.CommitTransactionAsync();

		}
		catch (Exception)
		{

			await session.AbortTransactionAsync();
			throw;

		}

	}

}