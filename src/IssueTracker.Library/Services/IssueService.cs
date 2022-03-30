using IssueTracker.Library.Contracts;
using IssueTracker.Library.Models;

using Microsoft.Extensions.Caching.Memory;

namespace IssueTracker.Library.Services;

public class IssueService : IIssueService
{
	private readonly IIssueRepository _repository;
	private readonly IMemoryCache _cache;
	private const string _cacheName = "IssueData";

	/// <summary>
	/// IssueService
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="cache"></param>
	public IssueService(IIssueRepository repository, IMemoryCache cache)
	{
		_repository = repository;
		_cache = cache;
	}

	/// <summary>
	/// CreateIssue
	/// </summary>
	/// <param name="issue"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateIssue(Issue issue)
	{
		if (issue == null)
		{
			throw new ArgumentNullException(nameof(issue));
		}

		await _repository.CreateIssue(issue);
	}

	/// <summary>
	/// GetIssue
	/// </summary>
	/// <param name="id"></param>
	/// <returns>An Issue</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<Issue> GetIssue(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var results = await _repository.GetIssue(id);

		return results;
	}

	/// <summary>
	/// GetAllIssues
	/// </summary>
	/// <returns>A List of All Issues</returns>
	public async Task<List<Issue>> GetIssues()
	{
		var output = _cache.Get<List<Issue>>(_cacheName);

		if (output is not null)
		{
			return output;
		}

		var results = await _repository.GetIssues();

		output = results.ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	/// GetUsersIssues
	/// </summary>
	/// <param name="userId"></param>
	/// <returns>A list of User Issues</returns>
	public async Task<List<Issue>> GetUsersIssues(string userId)
	{
		if (string.IsNullOrWhiteSpace(userId))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(userId));
		}

		var output = _cache.Get<List<Issue>>(userId);

		if (output is not null)
		{
			return output;
		}

		var results = await _repository.GetUsersIssues(userId);
		
		output = results.ToList();

		_cache.Set(userId, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	/// UpdateIssue
	/// </summary>
	/// <param name="issue"></param>
	public async Task UpdateIssue(Issue issue)
	{
		if (issue == null)
		{
			throw new ArgumentNullException(nameof(issue));
		}

		await _repository.UpdateIssue(issue.Id, issue);
		
		_cache.Remove(_cacheName);
	}
}