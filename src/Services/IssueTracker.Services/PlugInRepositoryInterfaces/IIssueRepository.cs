// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IIssueRepository.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

/// <summary>
/// Provides data access operations for issue entities in the data store.
/// </summary>
public interface IIssueRepository
{
	/// <summary>
	/// Archives an issue by marking it as inactive in the data store.
	/// </summary>
	/// <param name="issue">The issue to archive.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="issue"/> is null.</exception>
	Task ArchiveAsync(IssueModel issue);

	/// <summary>
	/// Creates a new issue in the data store.
	/// </summary>
	/// <param name="issue">The issue to create.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="issue"/> is null.</exception>
	Task CreateAsync(IssueModel issue);

	/// <summary>
	/// Retrieves a specific issue from the data store by its unique identifier.
	/// </summary>
	/// <param name="itemId">The unique identifier of the issue.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="IssueModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="itemId"/> is null or empty.</exception>
	Task<IssueModel> GetAsync(string itemId);

	/// <summary>
	/// Retrieves all issues from the data store.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains an enumerable collection of all <see cref="IssueModel"/> instances.
	/// </returns>
	Task<IEnumerable<IssueModel>> GetAllAsync();

	/// <summary>
	/// Retrieves all issues that have been approved for release.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains an enumerable collection of approved <see cref="IssueModel"/> instances.
	/// </returns>
	Task<IEnumerable<IssueModel>> GetApprovedAsync();

	/// <summary>
	/// Retrieves all issues created by a specific user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains an enumerable collection of <see cref="IssueModel"/> instances for the specified user.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> is null or empty.</exception>
	Task<IEnumerable<IssueModel>> GetByUserAsync(string userId);

	/// <summary>
	/// Retrieves all issues waiting for approval.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains an enumerable collection of <see cref="IssueModel"/> instances pending approval.
	/// </returns>
	Task<IEnumerable<IssueModel>> GetWaitingForApprovalAsync();

	/// <summary>
	/// Updates an existing issue in the data store.
	/// </summary>
	/// <param name="itemId">The unique identifier of the issue to update.</param>
	/// <param name="issue">The updated issue data.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="itemId"/> is null or empty.</exception>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="issue"/> is null.</exception>
	Task UpdateAsync(string itemId, IssueModel issue);
}
