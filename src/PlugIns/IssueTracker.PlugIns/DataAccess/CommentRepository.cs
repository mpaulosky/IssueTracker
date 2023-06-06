//-----------------------------------------------------------------------// <copyright file="CommentRepository.cs" company="mpaulosky">//		Author:  Matthew Paulosky//		Copyright (c) 2022. All rights reserved.// </copyright>//-----------------------------------------------------------------------namespace IssueTracker.PlugIns.DataAccess;/// <summary>
///   CommentRepository class
/// </summary>
public class CommentRepository : ICommentRepository
{
	private readonly IMongoCollection<CommentModel> _commentCollection;	/// <summary>
	///   CommentRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CommentRepository(IMongoDbContextFactory context)
	{
		ArgumentNullException.ThrowIfNull(context);

		var commentCollectionName = GetCollectionName(nameof(CommentModel));

		_commentCollection = context.GetCollection<CommentModel>(commentCollectionName);
	}	/// <summary>
	///   Archive the comment by setting the Archived property to true
	/// </summary>
	/// <param name="comment"></param>
	/// <returns>Task</returns>
	public async Task ArchiveAsync(CommentModel comment)
	{		// Archive the category																comment.Archived = true;

		await UpdateAsync(comment.Id, comment);
	}	/// <summary>
	///   CreateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="Exception"></exception>
	public async Task CreateAsync(CommentModel comment)
	{
		await _commentCollection.InsertOneAsync(comment);
	}	/// <summary>
	///   GetComment method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of CommentModel</returns>
	public async Task<CommentModel> GetAsync(string itemId)
	{
		var objectId = new ObjectId(itemId);

		var filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		var result = (await _commentCollection.FindAsync(filter)).FirstOrDefault();

		return result;
	}	/// <summary>
	///   GetComments method
	/// </summary>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>?> GetAllAsync()
	{
		var filter = Builders<CommentModel>.Filter.Empty;

		var results = (await _commentCollection.FindAsync(filter)).ToList();

		return results;
	}	/// <summary>
	///   GetCommentsBySource method
	/// </summary>
	/// <param name="source">BasicCommentOnSourceModel</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetBySourceAsync(BasicCommentOnSourceModel source)
	{
		var results = (await _commentCollection
				.FindAsync(s => s.CommentOnSource!.Id == source.Id && s.CommentOnSource.SourceType == source.SourceType))
			.ToList();

		return results;
	}	/// <summary>
	///   GetCommentsByUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable CommentModel</returns>
	public async Task<IEnumerable<CommentModel>> GetByUserAsync(string userId)
	{
		var results = (await _commentCollection.FindAsync(s => s.Author.Id == userId)).ToList();

		return results;
	}	/// <summary>
	///   UpdateComment method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="comment">CommentModel</param>
	public async Task UpdateAsync(string itemId, CommentModel comment)
	{
		var objectId = new ObjectId(itemId);

		var filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		await _commentCollection.ReplaceOneAsync(filter, comment);
	}	/// <summary>
	///   Up vote Comment method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="userId">string</param>
	/// <exception cref="Exception"></exception>
	public async Task UpVoteAsync(string itemId, string userId)
	{
		var objectId = new ObjectId(itemId);

		var filterComment = Builders<CommentModel>.Filter.Eq("_id", objectId);

		var comment = (await _commentCollection.FindAsync(filterComment)).FirstOrDefault();

		var isUpVote = comment.UserVotes.Add(userId);

		if (!isUpVote)
		{
			comment.UserVotes.Remove(userId);
		}

		await _commentCollection.ReplaceOneAsync(s => s.Id == itemId, comment);
	}
}
