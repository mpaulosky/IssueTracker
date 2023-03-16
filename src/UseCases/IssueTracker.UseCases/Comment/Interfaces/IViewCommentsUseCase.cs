//-----------------------------------------------------------------------
// <copyright File="IViewCommentsUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment.Interfaces;

public interface IViewCommentsUseCase
{
	Task<IEnumerable<CommentModel>> Execute();
}