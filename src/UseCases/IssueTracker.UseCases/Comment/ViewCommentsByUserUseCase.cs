//-----------------------------------------------------------------------
// <copyright File="ViewCommentsByUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class ViewCommentsByUserUseCase : IViewCommentsByUserUseCase
{

	private readonly ICommentRepository _commentRepository;

	public ViewCommentsByUserUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task<IEnumerable<CommentModel>?> ExecuteAsync(UserModel user)
	{

		Guard.Against.Null(user, nameof(user));

		return await _commentRepository.GetByUserAsync(user.Id);

	}

}
