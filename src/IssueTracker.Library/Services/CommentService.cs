﻿namespace IssueTracker.Library.Services;

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

		await _repository.CreateComment(comment);
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

		var result = await _repository.GetComment(id);

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

		var results = await _repository.GetComments();

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

		var results = await _repository.GetUsersComments(userId);

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

		var results = await _repository.GetIssuesComments(issueId);
		
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

		await _repository.UpdateComment(comment.Id, comment);

		_cache.Remove(_cacheName);
	}

	/// <summary>
	/// UpvoteComment method
	/// </summary>
	/// <param name="commentId">string</param>
	/// <param name="userId">string</param>
	/// <exception cref="ArgumentException"></exception>
	public async Task UpvoteComment(string commentId, string userId)
	{

		if (string.IsNullOrWhiteSpace(commentId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(commentId));
		}

		if (string.IsNullOrWhiteSpace(userId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(userId));
		}

		await _repository.UpvoteComment(commentId, userId);

		_cache.Remove(_cacheName);

	}
}