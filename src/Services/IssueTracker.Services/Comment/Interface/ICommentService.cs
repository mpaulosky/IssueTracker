﻿//-----------------------------------------------------------------------
// <copyright file="ICommentService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Services.Comment.Interface;

public interface ICommentService
{
	Task CreateComment(CommentModel comment);

	Task<CommentModel> GetComment(string commentId);

	Task<List<CommentModel>> GetComments();

	Task<List<CommentModel>> GetCommentsByUser(string userId);

	Task<List<CommentModel>> GetCommentsBySource(BasicCommentOnSourceModel source);

	Task UpdateComment(CommentModel comment);

	Task UpVoteComment(string commentId, string userId);
}
