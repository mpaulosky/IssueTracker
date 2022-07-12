using Microsoft.Extensions.Caching.Memory;

namespace IssueTracker.Library.Services;

public class CommentService : ICommentService
{
	private readonly ICommentRepository _repository;
	private readonly IMemoryCache _cache;
	private const string _cacheName = "CommentData";

	public CommentService(ICommentRepository repository, IMemoryCache cache)
	{
		_repository = repository;
		_cache = cache;
	}

	public async Task CreateComment(CommentModel comment)
	{
		if (comment == null)
		{
			throw new ArgumentNullException(nameof(comment));
		}

		await _repository.CreateComment(comment);
	}

	public async Task<CommentModel> GetComment(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var result = await _repository.GetComment(id);

		return result;
	}
	
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

	public async Task<List<CommentModel>> GetUsersComments(string userId)
	{
		if (string.IsNullOrWhiteSpace(userId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(userId));
		}

		var output = _cache.Get<List<CommentModel>>(userId);

		if (output is not null)
		{
			return output;
		}

		var results = await _repository.GetUsersComments(userId);

		output = results.ToList();

		_cache.Set(userId, output, TimeSpan.FromMinutes(1));

		return output;
	}

	public async Task UpdateComment(CommentModel comment)
	{
		if (comment == null)
		{
			throw new ArgumentNullException(nameof(comment));
		}

		await _repository.UpdateComment(comment.Id, comment);

		_cache.Remove(_cacheName);
	}

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