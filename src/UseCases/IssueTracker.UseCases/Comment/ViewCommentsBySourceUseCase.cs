//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewCommentsByIssueIdUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class ViewCommentsBySourceUseCase : IViewCommentsBySourceUseCase
{

	private readonly ICommentRepository _commentRepository;

	public ViewCommentsBySourceUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task<IEnumerable<CommentModel>?> ExecuteAsync(BasicCommentOnSourceModel? source)
	{
		if (source is null) return null;

		return await _commentRepository.GetCommentsBySourceAsync(source);

	}

}
