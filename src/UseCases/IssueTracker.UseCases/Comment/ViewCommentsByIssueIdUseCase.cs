//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewCommentsByIssueIdUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class ViewCommentsByIssueIdUseCase : IViewCommentsByIssueIdUseCase
{

	private readonly ICommentRepository _commentRepository;

	public ViewCommentsByIssueIdUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task<IEnumerable<CommentModel>?> ExecuteAsync(string? issueId)
	{

		if (string.IsNullOrWhiteSpace(issueId)) return null;

		return await _commentRepository.GetCommentsByIssueIdAsync(issueId);

	}

}
