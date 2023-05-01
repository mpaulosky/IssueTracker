//-----------------------------------------------------------------------
// <copyright>
//	File:		IArchiveCommentUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment.Interfaces;

public interface IArchiveCommentUseCase
{

	Task ExecuteAsync(CommentModel comment);

}
