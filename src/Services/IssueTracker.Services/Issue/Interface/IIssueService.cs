// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IIssueService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.Issue.Interface;

/// <summary>
/// Provides operations for managing issues in the system.
/// </summary>
public interface IIssueService
{
	/// <summary>
	/// Archives an existing issue, marking it as inactive.
	/// </summary>
	/// <param name="issue">The issue to archive.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="issue"/> is null.</exception>
	Task ArchiveIssue(IssueModel issue);

	/// <summary>
	/// Creates a new issue in the system.
	/// </summary>
	/// <param name="issue">The issue to create.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="issue"/> is null.</exception>
	Task CreateIssue(IssueModel issue);

	/// <summary>
	/// Retrieves a specific issue by its unique identifier.
	/// </summary>
	/// <param name="issueId">The unique identifier of the issue.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="IssueModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="issueId"/> is null or empty.</exception>
	Task<IssueModel> GetIssue(string? issueId);

	/// <summary>
	/// Retrieves all issues from the system with caching support.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of all <see cref="IssueModel"/> instances.
	/// </returns>
	/// <remarks>
	/// This method uses in-memory caching to improve performance.
	/// </remarks>
	Task<List<IssueModel>> GetIssues();

	/// <summary>
	/// Retrieves all issues created by a specific user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of <see cref="IssueModel"/> instances for the specified user.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> is null or empty.</exception>
	/// <remarks>
	/// This method uses in-memory caching to improve performance.
	/// </remarks>
	Task<List<IssueModel>> GetIssuesByUser(string userId);

	/// <summary>
	/// Retrieves all issues that have been approved for release.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of approved <see cref="IssueModel"/> instances.
	/// </returns>
	Task<List<IssueModel>> GetApprovedIssues();

	/// <summary>
	/// Retrieves all issues that are waiting for approval.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of <see cref="IssueModel"/> instances pending approval.
	/// </returns>
	Task<List<IssueModel>> GetIssuesWaitingForApproval();

	/// <summary>
	/// Updates an existing issue with new information.
	/// </summary>
	/// <param name="issue">The issue with updated information.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="issue"/> is null.</exception>
	Task UpdateIssue(IssueModel issue);
}
