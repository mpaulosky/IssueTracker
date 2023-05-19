//-----------------------------------------------------------------------
// <copyright>
//	File:		IUpVoteCommentUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment.Interfaces;

public interface IUpVoteCommentUseCase
{

	Task ExecuteAsync(CommentModel? comment, UserModel? user);

}
