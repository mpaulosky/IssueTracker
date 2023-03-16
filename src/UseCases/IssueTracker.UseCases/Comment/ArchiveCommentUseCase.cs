//-----------------------------------------------------------------------
// <copyright>
//	File:		ArchiveCommentUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class ArchiveCommentUseCase : IArchiveCommentUseCase
{

	private readonly ICommentRepository _commentRepository;

	public ArchiveCommentUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task Execute(CommentModel comment)
	{

		if (comment == null) return;

		// Set the comment to archived
		comment.Archived = true;

		await _commentRepository.UpdateCommentAsync(comment);

	}

}
