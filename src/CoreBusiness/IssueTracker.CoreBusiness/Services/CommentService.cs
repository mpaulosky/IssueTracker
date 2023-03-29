//-----------------------------------------------------------------------
// <copyright file="CommentService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.CoreBusiness.Contracts;
using IssueTracker.CoreBusiness.Services.Interfaces;

using Microsoft.Extensions.Caching.Memory;

namespace IssueTracker.CoreBusiness.Services;

/// <summary>
///		CommentService class
/// </summary>
public class CommentService : ICommentService
{
	private const string _cacheName = "CommentData";
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

		await _repository.CreateComment(comment);
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

		CommentModel result = await _repository.GetComment(commentId);

		return result;
	}

	/// <summary>
	///		GetComments method
	/// </summary>
	/// <returns>Task of List CommentModels</returns>
	public async Task<List<CommentModel>> GetComments()
	{
		List<CommentModel>? output = _cache.Get<List<CommentModel>>(_cacheName);

		if (output is not null) return output;

		IEnumerable<CommentModel> results = await _repository.GetComments();

		output = results.Where(x => !x.Archived).ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromMinutes(1));

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

		IEnumerable<CommentModel> results = await _repository.GetCommentsByUser(userId);

		return results.ToList();
	}

	/// <summary>
	///		GetCommentsByIssue method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <returns>Task of List CommentModels</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<List<CommentModel>> GetCommentsByIssue(string issueId)
	{
		Guard.Against.NullOrWhiteSpace(issueId, nameof(issueId));

		IEnumerable<CommentModel> results = await _repository.GetCommentsByIssue(issueId);

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

		await _repository.UpdateComment(comment.Id!, comment);

		_cache.Remove(_cacheName);
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

		await _repository.UpVoteComment(commentId, userId);

		_cache.Remove(_cacheName);
	}
}