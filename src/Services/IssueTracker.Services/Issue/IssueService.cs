// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IssueService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.Issue;

/// <summary>
///   IssueService class
/// </summary>
public class IssueService(IIssueRepository repository, IMemoryCache cache) : IIssueService
{
	private const string CacheName = "IssueData";

	/// <summary>
	///   ArchiveIssue method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task ArchiveIssue(IssueModel issue)
	{
		ArgumentNullException.ThrowIfNull(issue);

		cache.Remove(CacheName);

		return repository.ArchiveAsync(issue);
	}

	/// <summary>
	///   CreateIssue method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateIssue(IssueModel issue)
	{
		ArgumentNullException.ThrowIfNull(issue);

		await repository.CreateAsync(issue);
	}

	/// <summary>
	///   GetIssue method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <param name="viewerId">The Id of the User asking for the Issue</param>
	/// <param name="viewerIsAdmin">Whether that User is an Admin</param>
	/// <returns>Task of IssueModel, or null when the Issue doesn't exist or the viewer may not see it</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<IssueModel?> GetIssue(string? issueId, string? viewerId, bool viewerIsAdmin)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueId);

		IssueModel? result = await repository.GetAsync(issueId);

		if (result is null || !IsVisibleTo(result, viewerId, viewerIsAdmin))
		{
			return null;
		}

		return result;
	}

	/// <summary>
	///   Every User sees an approved Issue; a Pending or Rejected one is shown only to its Author and the Admins.
	/// </summary>
	private static bool IsVisibleTo(IssueModel issue, string? viewerId, bool viewerIsAdmin)
	{
		if (issue.ApprovedForRelease && !issue.Rejected)
		{
			return true;
		}

		return viewerIsAdmin || (!string.IsNullOrEmpty(viewerId) && issue.Author.Id == viewerId);
	}

	/// <summary>
	///   GetIssues method
	/// </summary>
	/// <returns>Task of List IssueModels</returns>
	public async Task<List<IssueModel>> GetIssues()
	{
		List<IssueModel>? output = cache.Get<List<IssueModel>>(CacheName);

		if (output is not null)
		{
			return output;
		}

		IEnumerable<IssueModel> results = await repository.GetAllAsync();

		output = results.ToList();

		cache.Set(CacheName, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	///   GetIssuesByUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of List IssueModels</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<List<IssueModel>> GetIssuesByUser(string userId)
	{
		ArgumentException.ThrowIfNullOrEmpty(userId);

		List<IssueModel>? output = cache.Get<List<IssueModel>>(userId);

		if (output is not null)
		{
			return output;
		}

		IEnumerable<IssueModel> results = await repository.GetByUserAsync(userId);

		output = results.ToList();

		cache.Set(userId, output, TimeSpan.FromMinutes(1));

		return output;
	}

	/// <summary>
	///   GetIssuesWaitingForApproval method
	/// </summary>
	/// <returns>Task of List IssueModels</returns>
	public async Task<List<IssueModel>> GetIssuesWaitingForApproval()
	{
		IEnumerable<IssueModel> results = await repository.GetWaitingForApprovalAsync();

		return results.ToList();
	}

	/// <summary>
	///   GetApprovedIssues method
	/// </summary>
	/// <returns>Task of List IssueModels</returns>
	public async Task<List<IssueModel>> GetApprovedIssues()
	{
		IEnumerable<IssueModel> results = await repository.GetApprovedAsync();

		return results.ToList();
	}

	/// <summary>
	///   UpdateIssue
	/// </summary>
	/// <param name="issue">IssueModel</param>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpdateIssue(IssueModel issue)
	{
		ArgumentNullException.ThrowIfNull(issue);

		await repository.UpdateAsync(issue.Id, issue);

		cache.Remove(CacheName);
	}
}