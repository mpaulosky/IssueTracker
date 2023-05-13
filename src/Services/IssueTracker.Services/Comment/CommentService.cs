//-----------------------------------------------------------------------
// <copyright file="CommentService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Services.Comment;

/// <summary>
///		CommentService class
/// </summary>
public class CommentService : ICommentService
{
	private const string CacheName = "CommentData";
	private readonly IMemoryCache _cache;
	private readonly ICommentRepository _repository;

	/// <summary>
	///		CommentService constructor
	/// </summary>
	/// <param name="repository">ICommentRepository</param>
	/// <param name="cache">IMemoryCache</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CommentService(ICommentRepository repository, IMemoryCache cache)
	{
		_repository = Guard.Against.Null(repository, nameof(repository));
		_cache = Guard.Against.Null(cache, nameof(cache));
	}

	/// <summary>
	///		CreateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateComment(CommentModel comment)
	{
		Guard.Against.Null(comment, nameof(comment));

		await _repository.CreateAsync(comment);
	}

	/// <summary>
	///		GetComment method
	/// </summary>
	/// <param name="commentId">string</param>
	/// <returns>Task of CommentModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<CommentModel> GetComment(string commentId)
	{
		Guard.Against.NullOrWhiteSpace(commentId, nameof(commentId));

		CommentModel result = await _repository.GetAsync(commentId);

		return result;
	}

	/// <summary>
	///		GetComments method
	/// </summary>
	/// <returns>Task of List CommentModels</returns>
	public async Task<List<CommentModel>> GetComments()
	{
		List<CommentModel>? output = _cache.Get<List<CommentModel>>(CacheName);

		if (output is not null) return output;

		IEnumerable<CommentModel>? results = await _repository.GetAllAsync();

		output = results!.Where(x => !x.Archived).ToList();

		_cache.Set(CacheName, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	///		GetCommentsByUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of List CommentModels</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<List<CommentModel>> GetCommentsByUser(string userId)
	{
		Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

		IEnumerable<CommentModel> results = await _repository.GetByUserAsync(userId);

		return results.ToList();
	}

	/// <summary>
	///		GetCommentsByIssue method
	/// </summary>
	/// <param name="source">BasicCommentOnSourceModel</param>
	/// <returns>Task of List CommentModels</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<List<CommentModel>> GetCommentsBySource(BasicCommentOnSourceModel source)
	{
		Guard.Against.Null(source, nameof(source));

		IEnumerable<CommentModel> results = await _repository.GetBySourceAsync(source);

		return results.ToList();
	}

	/// <summary>
	///		UpdateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpdateComment(CommentModel comment)
	{

		Guard.Against.Null(comment, nameof(comment));

		await _repository.UpdateAsync(comment.Id, comment);

		_cache.Remove(CacheName);

	}

	/// <summary>
	///		UpvoteComment method
	/// </summary>
	/// <param name="commentId">string</param>
	/// <param name="userId">string</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpVoteComment(string commentId, string userId)
	{
		Guard.Against.NullOrWhiteSpace(commentId, nameof(commentId));

		Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

		await _repository.UpVoteAsync(commentId, userId);

		_cache.Remove(CacheName);
	}
}
