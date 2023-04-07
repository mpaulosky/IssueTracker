//-----------------------------------------------------------------------
// <copyright file="IssueService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Services;

/// <summary>
///		IssueService class
/// </summary>
public class IssueService : IIssueService
{
	private const string _cacheName = "IssueData";
	private readonly IMemoryCache _cache;
	private readonly IIssueRepository _repository;

	/// <summary>
	///		IssueService
	/// </summary>
	/// <param name="repository">IIssueRepository</param>
	/// <param name="cache">IMemoryCache</param>
	/// <exception cref="ArgumentNullException"></exception>
	public IssueService(IIssueRepository repository, IMemoryCache cache)
	{
		_repository = Guard.Against.Null(repository, nameof(repository));
		_cache = Guard.Against.Null(cache, nameof(cache));
	}

	/// <summary>
	///		CreateIssue method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateIssue(IssueModel issue)
	{
		Guard.Against.Null(issue, nameof(issue));

		await _repository.CreateIssueAsync(issue);
	}

	/// <summary>
	///		GetIssue method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <returns>Task of IssueModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<IssueModel> GetIssue(string issueId)
	{
		Guard.Against.NullOrWhiteSpace(issueId, nameof(issueId));

		IssueModel results = await _repository.GetIssueAsync(issueId);

		return results;
	}

	/// <summary>
	///		GetIssues method
	/// </summary>
	/// <returns>Task of List IssueModels</returns>
	public async Task<List<IssueModel>> GetIssues()
	{
		List<IssueModel>? output = _cache.Get<List<IssueModel>>(_cacheName);

		if (output is not null) return output;

		IEnumerable<IssueModel> results = await _repository.GetIssuesAsync();

		output = results.ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	///		GetIssuesByUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of List IssueModels</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<List<IssueModel>> GetIssuesByUser(string userId)
	{

		Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

		List<IssueModel>? output = _cache.Get<List<IssueModel>>(userId);

		if (output is not null) return output;

		IEnumerable<IssueModel> results = await _repository.GetIssuesByUserAsync(userId);

		output = results.ToList();

		_cache.Set(userId, output, TimeSpan.FromMinutes(1));

		return output;

	}

	/// <summary>
	///		UpdateIssue
	/// </summary>
	/// <param name="issue">IssueModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpdateIssue(IssueModel issue)
	{

		Guard.Against.Null(issue, nameof(issue));

		await _repository.UpdateIssueAsync(issue.Id!, issue);

		_cache.Remove(_cacheName);

	}

	/// <summary>
	///		GetIssuesWaitingForApproval method
	/// </summary>
	/// <returns>Task of List IssueModels</returns>
	public async Task<List<IssueModel>> GetIssuesWaitingForApproval()
	{

		IEnumerable<IssueModel> results = await _repository.GetIssuesWaitingForApprovalAsync();

		return results.ToList();

	}

	/// <summary>
	///		GetApprovedIssues method
	/// </summary>
	/// <returns>Task of List IssueModels</returns>
	public async Task<List<IssueModel>> GetApprovedIssues()
	{

		IEnumerable<IssueModel> results = await _repository.GetApprovedIssuesAsync();

		return results.ToList();

	}

}