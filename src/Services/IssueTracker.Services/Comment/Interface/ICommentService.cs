// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ICommentService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.Comment.Interface;

/// <summary>
/// Provides operations for managing comments on issues in the system.
/// </summary>
public interface ICommentService
{
	/// <summary>
	/// Archives an existing comment, marking it as inactive.
	/// </summary>
	/// <param name="comment">The comment to archive.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="comment"/> is null.</exception>
	Task ArchiveComment(CommentModel comment);

	/// <summary>
	/// Creates a new comment in the system.
	/// </summary>
	/// <param name="comment">The comment to create.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="comment"/> is null.</exception>
	Task CreateComment(CommentModel comment);

	/// <summary>
	/// Retrieves a specific comment by its unique identifier.
	/// </summary>
	/// <param name="commentId">The unique identifier of the comment.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="CommentModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="commentId"/> is null or empty.</exception>
	Task<CommentModel> GetComment(string commentId);

	/// <summary>
	/// Retrieves all comments from the system with caching support.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of all <see cref="CommentModel"/> instances.
	/// </returns>
	/// <remarks>
	/// This method uses in-memory caching to improve performance.
	/// </remarks>
	Task<List<CommentModel>> GetComments();

	/// <summary>
	/// Retrieves all comments created by a specific user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of <see cref="CommentModel"/> instances for the specified user.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> is null or empty.</exception>
	Task<List<CommentModel>> GetCommentsByUser(string userId);

	/// <summary>
	/// Retrieves all comments associated with a specific issue.
	/// </summary>
	/// <param name="issue">The issue to retrieve comments for.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of <see cref="CommentModel"/> instances for the specified issue.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="issue"/> is null.</exception>
	Task<List<CommentModel>> GetCommentsByIssue(BasicIssueModel issue);

	/// <summary>
	/// Updates an existing comment with new information.
	/// </summary>
	/// <param name="comment">The comment with updated information.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="comment"/> is null.</exception>
	Task UpdateComment(CommentModel comment);

	/// <summary>
	/// Registers an upvote for a comment by a specific user.
	/// </summary>
	/// <param name="commentId">The unique identifier of the comment.</param>
	/// <param name="userId">The unique identifier of the user upvoting.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="commentId"/> or <paramref name="userId"/> is null or empty.</exception>
	Task UpVoteComment(string commentId, string userId);
}
