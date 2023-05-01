//-----------------------------------------------------------------------
// <copyright File="CreateNewCommentUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class CreateCommentUseCase : ICreateCommentUseCase
{

	private readonly ICommentRepository _commentRepository;

	public CreateCommentUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task ExecuteAsync(CommentModel comment)
	{

		Guard.Against.Null(comment, nameof(comment));

		await _commentRepository.CreateAsync(comment);

	}

}
