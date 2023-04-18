//-----------------------------------------------------------------------
// <copyright File="ICreateNewCommentUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment.Interfaces;

public interface ICreateNewCommentUseCase
{
	Task Execute(CommentModel comment);
}
