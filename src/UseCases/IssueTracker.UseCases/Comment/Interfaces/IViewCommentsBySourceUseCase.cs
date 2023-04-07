//-----------------------------------------------------------------------
// <copyright File="IViewCommentsByIssueIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment.Interfaces;

public interface IViewCommentsBySourceUseCase
{

	Task<IEnumerable<CommentModel>?> ExecuteAsync(BasicCommentOnSourceModel source);

}
