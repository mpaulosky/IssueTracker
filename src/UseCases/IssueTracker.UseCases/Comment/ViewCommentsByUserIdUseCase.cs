//-----------------------------------------------------------------------
// <copyright File="ViewCommentsByUserIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class ViewCommentsByUserIdUseCase : IViewCommentsByUserIdUseCase
{

	private readonly ICommentRepository _commentRepository;

	public ViewCommentsByUserIdUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task<IEnumerable<CommentModel>?> ExecuteAsync(UserModel? user)
	{

		if (user == null) return null;

		return await _commentRepository.GetCommentsByUserIdAsync(user.Id);

	}

}
