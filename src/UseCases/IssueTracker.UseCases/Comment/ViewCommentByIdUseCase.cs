//-----------------------------------------------------------------------
// <copyright File="ViewCommentByIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UseCases.PlugInRepositoryInterfaces;

namespace IssueTracker.UseCases.Comment;

public class ViewCommentByIdUseCase : IViewCommentByIdUseCase
{

	private readonly ICommentRepository _commentRepository;

	public ViewCommentByIdUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task<CommentModel> Execute(string commentId)
	{

		return await _commentRepository.ViewCommentByIdAsync(commentId);

	}
}
