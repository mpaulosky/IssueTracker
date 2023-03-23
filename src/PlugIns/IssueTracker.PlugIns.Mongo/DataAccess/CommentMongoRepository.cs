//-----------------------------------------------------------------------
// <copyright>
//	File:		CommentMongoRepository.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Mongo.DataAccess;

/// <summary>
///		CommentRepository class
/// </summary>
public class CommentMongoRepository : ICommentRepository
{

	private readonly IMongoCollection<CommentModel> _commentCollection;

	/// <summary>
	///		CommentRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CommentMongoRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var commentCollectionName = GetCollectionName(nameof(CommentModel));

		_commentCollection = context.GetCollection<CommentModel>(commentCollectionName);

	}

	/// <summary>
	///		CreateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="Exception"></exception>
	public async Task CreateCommentAsync(CommentModel comment)
	{

		await _commentCollection.InsertOneAsync(comment);

	}

	/// <summary>
	///		GetComment method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of CommentModel</returns>
	public async Task<CommentModel> GetCommentByIdAsync(string itemId)
	{

		var objectId = new ObjectId(itemId);

		FilterDefinition<CommentModel> filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		CommentModel result = (await _commentCollection.FindAsync(filter)).FirstOrDefault();

		return result;

	}

	/// <summary>
	///		GetComments method
	/// </summary>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetCommentsAsync()
	{

		FilterDefinition<CommentModel> filter = Builders<CommentModel>.Filter.Empty;

		var results = (await _commentCollection.FindAsync(filter)).ToList();

		return results;

	}

	/// <summary>
	///		GetCommentsByIssue method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetCommentsByIssueIdAsync(string issueId)
	{

		var results = (await _commentCollection.FindAsync(s => s.Source.Id == issueId)).ToList();

		return results;

	}

	/// <summary>
	///		GetCommentsByUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetCommentsByUserIdAsync(string userId)
	{

		var results = (await _commentCollection.FindAsync(s => s.Author.Id == userId)).ToList();

		return results;

	}

	/// <summary>
	///		UpdateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	public async Task UpdateCommentAsync(CommentModel comment)
	{

		var objectId = new ObjectId(comment.Id);

		FilterDefinition<CommentModel> filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		await _commentCollection.ReplaceOneAsync(filter, comment);

	}

	/// <summary>
	///		UpvoteComment method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="userId">string</param>
	/// <exception cref="Exception"></exception>
	public async Task UpVoteCommentAsync(string itemId, string userId)
	{
		var objectId = new ObjectId(itemId);

		FilterDefinition<CommentModel> filterComment = Builders<CommentModel>.Filter.Eq("_id", objectId);

		CommentModel comment = (await _commentCollection.FindAsync(filterComment)).FirstOrDefault();

		var isUpvote = comment.UserVotes.Add(userId);

		if (isUpvote == false) comment.UserVotes.Remove(userId);

		await _commentCollection.ReplaceOneAsync(s => s.Id == itemId, comment);

	}

}