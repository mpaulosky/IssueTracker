//-----------------------------------------------------------------------
// <copyright file="ICommentRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface ICommentRepository
{
	Task<CommentModel> GetComment(string itemId);

	Task<IEnumerable<CommentModel>> GetComments();

	Task UpdateComment(string itemId, CommentModel comment);

	Task<IEnumerable<CommentModel>> GetUsersComments(string userId);

	Task<IEnumerable<CommentModel>> GetIssuesComments(string issueId);

	Task CreateComment(CommentModel comment);

	Task UpVoteComment(string itemId, string userId);
}