//-----------------------------------------------------------------------
// <copyright File="IViewCommentByIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment.Interfaces;

public interface IViewCommentByIdUseCase
{
	Task<CommentModel?> ExecuteAsync(string commentId);
}