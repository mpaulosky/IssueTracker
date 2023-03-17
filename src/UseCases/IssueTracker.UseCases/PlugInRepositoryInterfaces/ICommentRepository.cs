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
	Task CreateCommentAsync(CommentModel comment);
	Task UpdateCommentAsync(CommentModel comment);
	Task<CommentModel> GetCommentByIdAsync(string commentId);
	Task<IEnumerable<CommentModel>> GetCommentsAsync();
	Task<IEnumerable<CommentModel>> GetCommentsByIssueIdAsync(string id);
	Task<IEnumerable<CommentModel>> GetCommentsByUserIdAsync(string id);
}
