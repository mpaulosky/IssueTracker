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
public class CommentRepository : ICommentRepository
{

	private readonly IMongoCollection<CommentModel> _collection;

	/// <summary>
	///		CommentRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CommentRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var commentCollectionName = GetCollectionName(nameof(CommentModel));

		_collection = context.GetCollection<CommentModel>(commentCollectionName);

	}

	/// <summary>
	///		ArchiveAsync method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	public async Task ArchiveAsync(CommentModel comment)
	{

		// Archive the category
		comment.Archived = true;

		await UpdateAsync(comment);

	}
	/// <summary>
	///		CreateAsync method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	public async Task CreateAsync(CommentModel comment)
	{

		await _collection.InsertOneAsync(comment);

	}

	/// <summary>
	///		GetAsync method
	/// </summary>
	/// <param name="commentId">string</param>
	/// <returns>Task of CommentModel</returns>
	public async Task<CommentModel?> GetAsync(string? commentId)
	{

		return (await _collection
			.FindAsync(s => s.Id == commentId && !s.Archived))
			.FirstOrDefault();

	}

	/// <summary>
	///		GetAllAsync method
	/// </summary>
	/// <param name="includeArchived">bool</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>?> GetAllAsync(bool includeArchived = false)
	{

		if (includeArchived)
		{

			var filter = Builders<CommentModel>.Filter.Empty;
			return (await _collection
					.FindAsync(filter))
					.ToList();

		}
		else
		{

			return (await _collection
					.FindAsync(x => x.Archived == includeArchived))
					.ToList();

		}

	}

	/// <summary>
	///		GetBySourceAsync method
	/// </summary>
	/// <param name="source">BasicCommentOnSourceModel</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>?> GetBySourceAsync(BasicCommentOnSourceModel source)
	{

		return (await _collection
				.FindAsync(filter: s => s.CommentOnSource!.Id == s.Id && s.CommentOnSource.SourceType == source.SourceType && !s.Archived))
				.ToList();

	}

	/// <summary>
	///		GetByUserAsync method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>?> GetByUserAsync(string userId)
	{

		return (await _collection
				.FindAsync(s => s.Author.Id == userId && !s.Archived))
				.ToList();

	}

	/// <summary>
	///		UpdateAsync method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	public async Task UpdateAsync(CommentModel comment)
	{

		var objectId = new ObjectId(comment.Id);

		FilterDefinition<CommentModel> filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, comment);

	}

	/// <summary>
	///		UpvoteAsync method
	/// </summary>
	/// <param name="commentId">string</param>
	/// <param name="userId">string</param>
	/// <exception cref="Exception"></exception>
	public async Task UpVoteAsync(string? commentId, string userId)
	{
		var objectId = new ObjectId(commentId);

		FilterDefinition<CommentModel> filterComment = Builders<CommentModel>.Filter.Eq("_id", objectId);

		CommentModel comment = (await _collection.FindAsync(filterComment)).FirstOrDefault();

		var isUpvote = comment.UserVotes.Add(userId);

		if (!isUpvote) comment.UserVotes.Remove(userId);

		await _collection.ReplaceOneAsync(s => s.Id == commentId, comment);

	}

}
