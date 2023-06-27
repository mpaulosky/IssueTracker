// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CommentService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.Comment;

/// <summary>
///   CommentService class
/// </summary>
public class CommentService : ICommentService
{
	private const string CacheName = "CommentData";
	private readonly IMemoryCache _cache;
	private readonly ICommentRepository _repository;

	/// <summary>
	///   CommentService constructor
	/// </summary>
	/// <param name="repository">ICommentRepository</param>
	/// <param name="cache">IMemoryCache</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CommentService(ICommentRepository repository, IMemoryCache cache)
	{
		ArgumentNullException.ThrowIfNull(repository);
		ArgumentNullException.ThrowIfNull(cache);

		_repository = repository;
		_cache = cache;
	}

	/// <summary>
	///   ArchiveComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task ArchiveComment(CommentModel comment)
	{
		ArgumentNullException.ThrowIfNull(comment);

		_cache.Remove(CacheName);

		return _repository.ArchiveAsync(comment);
	}

	/// <summary>
	///   CreateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateComment(CommentModel comment)
	{
		ArgumentNullException.ThrowIfNull(comment);

		await _repository.CreateAsync(comment);
	}

	/// <summary>
	///   GetComment method
	/// </summary>
	/// <param name="commentId">string</param>
	/// <returns>Task of CommentModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<CommentModel> GetComment(string commentId)
	{
		ArgumentException.ThrowIfNullOrEmpty(commentId);

		CommentModel result = await _repository.GetAsync(commentId);

		return result;
	}

	/// <summary>
	///   GetComments method
	/// </summary>
	/// <returns>Task of List CommentModels</returns>
	public async Task<List<CommentModel>> GetComments()
	{
		List<CommentModel>? output = _cache.Get<List<CommentModel>>(CacheName);

		if (output is not null)
		{
			return output;
		}

		IEnumerable<CommentModel>? results = await _repository.GetAllAsync();

		output = results!.Where(x => !x.Archived).ToList();

		_cache.Set(CacheName, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	///   GetCommentsByUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of List CommentModels</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<List<CommentModel>> GetCommentsByUser(string userId)
	{
		ArgumentException.ThrowIfNullOrEmpty(userId);

		IEnumerable<CommentModel> results = await _repository.GetByUserAsync(userId);

		return results.ToList();
	}

	/// <summary>
	///   GetCommentsByIssue method
	/// </summary>
	/// <param name="issue">BasicIssueModel</param>
	/// <returns>Task of List CommentModels</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<List<CommentModel>> GetCommentsByIssue(BasicIssueModel issue)
	{
		ArgumentNullException.ThrowIfNull(issue);

		IEnumerable<CommentModel> results = await _repository.GetByIssueAsync(issue);

		return results.ToList();
	}

	/// <summary>
	///   UpdateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpdateComment(CommentModel comment)
	{
		ArgumentNullException.ThrowIfNull(comment);

		await _repository.UpdateAsync(comment.Id, comment);

		_cache.Remove(CacheName);
	}

	/// <summary>
	///   UpVoteComment method
	/// </summary>
	/// <param name="commentId">string</param>
	/// <param name="userId">string</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpVoteComment(string commentId, string userId)
	{
		ArgumentException.ThrowIfNullOrEmpty(commentId);

		ArgumentException.ThrowIfNullOrEmpty(userId);

		await _repository.UpVoteAsync(commentId, userId);

		_cache.Remove(CacheName);
	}
}