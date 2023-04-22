//-----------------------------------------------------------------------
// <copyright File="CreateNewCommentUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class CreateNewCommentUseCase : ICreateNewCommentUseCase
{

	private readonly ICommentRepository _commentRepository;

	public CreateNewCommentUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task ExecuteAsync(CommentModel? comment)
	{

		if (comment == null) return;

		await _commentRepository.CreateCommentAsync(comment);

	}

}
