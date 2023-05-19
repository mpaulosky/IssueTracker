//-----------------------------------------------------------------------
// <copyright>
//  File:    UpdateCommentUseCase.cs
//	Company: mpaulosky
//	Author:  Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;
public class UpdateCommentUseCase : IUpdateCommentUseCase
{

	private readonly ICommentRepository _commentRepository;

	public UpdateCommentUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task ExecuteAsync(CommentModel? comment)
	{

		ArgumentNullException.ThrowIfNull(comment);

		await _commentRepository.UpdateAsync(comment);

	}

}
