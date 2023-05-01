//-----------------------------------------------------------------------
// <copyright File="ViewCommentsUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class ViewCommentsUseCase : IViewCommentsUseCase
{

	private readonly ICommentRepository _commentRepository;

	public ViewCommentsUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task<IEnumerable<CommentModel>?> ExecuteAsync(bool includeArchive = false)
	{

		return await _commentRepository.GetAllAsync();

	}
}
