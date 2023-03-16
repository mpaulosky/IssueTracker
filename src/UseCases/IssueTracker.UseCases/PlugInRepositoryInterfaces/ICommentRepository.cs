//-----------------------------------------------------------------------
// <copyright File="ICommentRepository"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.PlugInRepositoryInterfaces;

public interface ICommentRepository
{
	Task CreateNewCommentAsync(CommentModel comment);
	Task UpdateCommentAsync(CommentModel comment);
	Task<CommentModel> ViewCommentByIdAsync(string commentId);
	Task<IEnumerable<CommentModel>> ViewCommentsAsync();
	Task<IEnumerable<CommentModel>> ViewCommentsByIssueIdAsync(string id);
	Task<IEnumerable<CommentModel>> ViewCommentsByUserIdAsync(string id);
}
