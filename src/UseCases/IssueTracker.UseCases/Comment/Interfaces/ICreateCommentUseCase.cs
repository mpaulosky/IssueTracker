//-----------------------------------------------------------------------
// <copyright File="ICreateNewCommentUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment.Interfaces;

public interface ICreateCommentUseCase
{

	Task ExecuteAsync(CommentModel comment);

}
