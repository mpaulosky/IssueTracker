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

	public async Task<IEnumerable<CommentModel>> ExecuteAsync(IssueModel issue)
	{

		if (issue == null)
		{
			return new List<CommentModel>();
		}

		return await _commentRepository.GetCommentsByIssueIdAsync(issue.Id);

	}

}
