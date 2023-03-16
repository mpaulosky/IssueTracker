//-----------------------------------------------------------------------
// <copyright File="ViewCommentsUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UseCases.PlugInRepositoryInterfaces;

namespace IssueTracker.UseCases.Comment;

public class ViewCommentsUseCase : IViewCommentsUseCase
{

	private readonly ICommentRepository _commentRepository;

	public ViewCommentsUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task<IEnumerable<CommentModel>> Execute()
	{

		return await _commentRepository.ViewCommentsAsync();

	}
}
