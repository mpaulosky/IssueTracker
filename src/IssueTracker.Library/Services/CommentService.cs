//-----------------------------------------------------------------------
// <copyright file="CommentService.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.Library.Services;

/// <summary>
/// CommentService class
/// </summary>
public class CommentService : ICommentService
{
	private const string _cacheName = "CommentData";
	private readonly IMemoryCache _cache;
	private readonly ICommentRepository _repository;

	/// <summary>
	/// CommentService constructor
	/// </summary>
	/// <param name="repository">ICommentRepository</param>
	/// <param name="cache">IMemoryCache</param>
	public CommentService(ICommentRepository repository, IMemoryCache cache)
	{
		_repository = repository;
		_cache = cache;
	}

	/// <summary>
	/// CreateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateComment(CommentModel comment)
	{
		if (comment == null)
		{
			throw new ArgumentNullException(nameof(comment));
		}

		await _repository.CreateComment(comment).ConfigureAwait(true);
	}

	/// <summary>
	/// GetComment method
	/// </summary>
	/// <param name="id">string</param>
	/// <returns>Task of CommentModel</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<CommentModel> GetComment(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var result = await _repository.GetComment(id).ConfigureAwait(true);

		return result;
	}

	/// <summary>
	/// GetComments method
	/// </summary>
	/// <returns>Task of List CommentModels</returns>
	public async Task<List<CommentModel>> GetComments()
	{
		var output = _cache.Get<List<CommentModel>>(_cacheName);

		if (output is not null)
		{
			return output;
		}

		var results = await _repository.GetComments().ConfigureAwait(true);

		output = results.Where(x => x.Archived == false).ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	/// GetUserComments method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of List CommentModels</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<List<CommentModel>> GetUsersComments(string userId)
	{
		if (string.IsNullOrWhiteSpace(userId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(userId));
		}

		//var output = _cache.Get<List<CommentModel>>(userId);

		//if (output is not null)
		//{
		//	return output;
		//}

		var results = await _repository.GetUsersComments(userId).ConfigureAwait(true);

		//output = results.ToList();

		//_cache.Set(userId, output, TimeSpan.FromMinutes(1));

		return results.ToList();
	}

	/// <summary>
	/// GetIssuesComments method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <returns>Task of List CommentModels</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<List<CommentModel>> GetIssuesComments(string issueId)
	{
		if (string.IsNullOrWhiteSpace(issueId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(issueId));
		}

		var results = await _repository.GetIssuesComments(issueId).ConfigureAwait(true);
		
		return results.ToList();
	}

	/// <summary>
	/// UpdateComment method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpdateComment(CommentModel comment)
	{
		if (comment == null)
		{
			throw new ArgumentNullException(nameof(comment));
		}

		await _repository.UpdateComment(comment.Id, comment).ConfigureAwait(true);

		_cache.Remove(_cacheName);
	}

	/// <summary>
	/// UpvoteComment method
	/// </summary>
	/// <param name="commentId">string</param>
	/// <param name="userId">string</param>
	/// <exception cref="ArgumentException"></exception>
	public async Task UpVoteComment(string commentId, string userId)
	{

		if (string.IsNullOrWhiteSpace(commentId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(commentId));
		}

		if (string.IsNullOrWhiteSpace(userId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(userId));
		}

		await _repository.UpVoteComment(commentId, userId).ConfigureAwait(true);

		_cache.Remove(_cacheName);

	}
}