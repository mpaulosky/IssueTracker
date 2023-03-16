//-----------------------------------------------------------------------
// <copyright File="ViewCommentsByUserIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment.Interfaces;

public interface IViewCommentsByUserIdUseCase
{
	Task<IEnumerable<CommentModel>> Execute(UserModel user);
}