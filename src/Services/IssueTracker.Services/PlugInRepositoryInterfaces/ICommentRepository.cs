// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ICommentRepository.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

/// <summary>
/// Provides data access operations for comment entities in the data store.
/// </summary>
public interface ICommentRepository
{
	/// <summary>
	/// Archives a comment by marking it as inactive in the data store.
	/// </summary>
	/// <param name="comment">The comment to archive.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="comment"/> is null.</exception>
	Task ArchiveAsync(CommentModel comment);

	/// <summary>
	/// Creates a new comment in the data store.
	/// </summary>
	/// <param name="comment">The comment to create.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="comment"/> is null.</exception>
	Task CreateAsync(CommentModel comment);

	/// <summary>
	/// Retrieves a specific comment from the data store by its unique identifier.
	/// </summary>
	/// <param name="itemId">The unique identifier of the comment.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="CommentModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="itemId"/> is null or empty.</exception>
	Task<CommentModel> GetAsync(string itemId);

	/// <summary>
	/// Retrieves all comments from the data store.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains an enumerable collection of all <see cref="CommentModel"/> instances,
	/// or null if no comments exist.
	/// </returns>
	Task<IEnumerable<CommentModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves all comments created by a specific user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains an enumerable collection of <see cref="CommentModel"/> instances for the specified user.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> is null or empty.</exception>
	Task<IEnumerable<CommentModel>> GetByUserAsync(string userId);

	/// <summary>
	/// Retrieves all comments associated with a specific issue.
	/// </summary>
	/// <param name="issue">The issue to retrieve comments for.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains an enumerable collection of <see cref="CommentModel"/> instances for the specified issue.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="issue"/> is null.</exception>
	Task<IEnumerable<CommentModel>> GetByIssueAsync(BasicIssueModel issue);

	/// <summary>
	/// Updates an existing comment in the data store.
	/// </summary>
	/// <param name="itemId">The unique identifier of the comment to update.</param>
	/// <param name="comment">The updated comment data.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="itemId"/> is null or empty.</exception>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="comment"/> is null.</exception>
	Task UpdateAsync(string itemId, CommentModel comment);

	/// <summary>
	/// Registers an upvote for a comment by a specific user.
	/// </summary>
	/// <param name="itemId">The unique identifier of the comment.</param>
	/// <param name="userId">The unique identifier of the user upvoting.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="itemId"/> or <paramref name="userId"/> is null or empty.</exception>
	Task UpVoteAsync(string itemId, string userId);
}
