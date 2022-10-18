﻿//-----------------------------------------------------------------------
// <copyright file="ICommentRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface ICommentRepository
{

	Task CreateComment(CommentModel comment);

	Task<CommentModel> GetComment(string itemId);

	Task<IEnumerable<CommentModel>> GetComments();

	Task<IEnumerable<CommentModel>> GetCommentsByUser(string userId);

	Task<IEnumerable<CommentModel>> GetCommentsByIssue(string issueId);

	Task UpdateComment(string itemId, CommentModel comment);

	Task UpVoteComment(string itemId, string userId);

}