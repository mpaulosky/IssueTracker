﻿//-----------------------------------------------------------------------
// <copyright file="CommentRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.DataAccess;

/// <summary>
///		CommentRepository class
/// </summary>
public class CommentRepository : ICommentRepository
{

	private readonly IMongoCollection<CommentModel> _commentCollection;
	private readonly IMongoDbContextFactory _context;

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

	}

	/// <summary>
	///		CreateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="Exception"></exception>
	public async Task CreateComment(CommentModel comment)
	{

		await _commentCollection.InsertOneAsync(comment);

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

		try
		{

			var objectId = new ObjectId(itemId);

			var filterComment = Builders<CommentModel>.Filter.Eq("_id", objectId);

			var comment = (await _commentCollection.FindAsync(filterComment)).FirstOrDefault();

			var isUpvote = comment.UserVotes.Add(userId);

			if (isUpvote == false)
			{

				comment.UserVotes.Remove(userId);

			}

			await _commentCollection.ReplaceOneAsync(s => s.Id == itemId, comment);

		}
		catch (Exception)
		{

			throw;

		}

	}

}