﻿//-----------------------------------------------------------------------
// <copyright file="CommentRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.DataAccess;

/// <summary>
///		CommentRepository class
/// </summary>
public class CommentRepository : ICommentRepository
{

	private readonly IMongoCollection<CommentModel> _commentCollection;

	/// <summary>
	///		CommentRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CommentRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var commentCollectionName = GetCollectionName(nameof(CommentModel));

		_commentCollection = context.GetCollection<CommentModel>(commentCollectionName);

	}

	/// <summary>
	/// Archive the comment by setting the Archived property to true
	/// </summary>
	/// <param name="comment"></param>
	/// <returns>Task</returns>
	public async Task ArchiveCommentAsync(CommentModel comment)
	{

		var objectId = new ObjectId(comment.Id);

		// Archive the comment
		comment.Archived = true;

		FilterDefinition<CommentModel> filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		await _commentCollection.ReplaceOneAsync(filter, comment);

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
	public async Task<CommentModel> GetCommentAsync(string itemId)
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
	public async Task<IEnumerable<CommentModel>?> GetCommentsAsync()
	{

		FilterDefinition<CommentModel> filter = Builders<CommentModel>.Filter.Empty;

		var results = (await _commentCollection.FindAsync(filter)).ToList();

		return results;

	}

	/// <summary>
	///		GetCommentsBySource method
	/// </summary>
	/// <param name="source">BasicCommentOnSourceModel</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetCommentsBySourceAsync(BasicCommentOnSourceModel source)
	{

		var results = (await _commentCollection
			.FindAsync(s => s.CommentOnSource!.Id == source.Id && s.CommentOnSource.SourceType == source.SourceType)).ToList();

		return results;

	}

	/// <summary>
	///		GetCommentsByUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetCommentsByUserAsync(string userId)
	{

		var results = (await _commentCollection.FindAsync(s => s.Author.Id == userId)).ToList();

		return results;

	}

	/// <summary>
	///		UpdateComment method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="comment">CommentModel</param>
	public async Task UpdateCommentAsync(string itemId, CommentModel comment)
	{

		var objectId = new ObjectId(itemId);

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

		if (!isUpvote) comment.UserVotes.Remove(userId);

		await _commentCollection.ReplaceOneAsync(s => s.Id == itemId, comment);

	}

}
