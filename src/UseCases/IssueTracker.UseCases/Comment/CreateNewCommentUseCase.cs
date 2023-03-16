//-----------------------------------------------------------------------
// <copyright File="CreateNewCommentUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UseCases.Comment.Interfaces;
using IssueTracker.UseCases.PlugInRepositoryInterfaces;

namespace IssueTracker.UseCases.Comment;

public class CreateNewCommentUseCase : ICreateNewCommentUseCase
{

	private readonly ICommentRepository _commentRepository;

	public CreateNewCommentUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task Execute(CommentModel comment)
	{

		if (comment == null) return;

		await _commentRepository.CreateNewCommentAsync(comment);

	}

}
