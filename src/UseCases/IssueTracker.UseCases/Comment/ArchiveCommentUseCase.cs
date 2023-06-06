﻿//-----------------------------------------------------------------------
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

	public async Task ExecuteAsync(CommentModel? comment)
	{

		ArgumentNullException.ThrowIfNull(comment);

		await _commentRepository.ArchiveAsync(comment);

	}

}