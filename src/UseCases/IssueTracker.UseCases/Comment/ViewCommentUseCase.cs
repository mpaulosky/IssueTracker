//-----------------------------------------------------------------------
// <copyright File="ViewCommentUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class ViewCommentUseCase : IViewCommentUseCase
{

	private readonly ICommentRepository _commentRepository;

	public ViewCommentUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task<CommentModel?> ExecuteAsync(string commentId)
	{

		Guard.Against.NullOrWhiteSpace(commentId, nameof(commentId));

		return await _commentRepository.GetAsync(commentId);

	}
}
