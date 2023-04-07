//-----------------------------------------------------------------------
// <copyright File="IViewCommentsByIssueIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment.Interfaces;

public interface IViewCommentsByIssueIdUseCase
{

	Task<IEnumerable<CommentModel>?> ExecuteAsync(string? issueId);

}