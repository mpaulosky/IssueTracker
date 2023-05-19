//-----------------------------------------------------------------------
// <copyright>
//	File:		UpVoteCommentUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Comment;

public class UpVoteCommentUseCase : IUpVoteCommentUseCase
{

	private readonly ICommentRepository _commentRepository;

	public UpVoteCommentUseCase(ICommentRepository commentRepository)
	{

		_commentRepository = commentRepository;

	}

	public async Task ExecuteAsync(CommentModel? comment, UserModel? user)
	{

		ArgumentNullException.ThrowIfNull(comment);
		ArgumentNullException.ThrowIfNull(user);

		await _commentRepository.UpVoteAsync(comment.Id, user.Id);

	}

}
