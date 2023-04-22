//-----------------------------------------------------------------------
// <copyright>
//  File:    EditCommentUseCase.cs
//	Company: mpaulosky
//	Author:  Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;
public class EditCommentUseCase : IEditCommentUseCase
{

	private readonly ICommentRepository _commentRepository;

	public EditCommentUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task ExecuteAsync(CommentModel? comment)
	{

		if (comment == null) return;

		await _commentRepository.UpdateCommentAsync(comment);

	}

}
