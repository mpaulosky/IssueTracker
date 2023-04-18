//-----------------------------------------------------------------------
// <copyright file="ICommentRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.PlugInRepositoryInterfaces;

public interface ICommentRepository
{

	Task ArchiveCommentAsync(CommentModel comment);

	Task CreateCommentAsync(CommentModel comment);

	Task<CommentModel> GetCommentAsync(string itemId);

	Task<IEnumerable<CommentModel>?> GetCommentsAsync();

	Task<IEnumerable<CommentModel>> GetCommentsByUserAsync(string userId);

	Task<IEnumerable<CommentModel>> GetCommentsBySourceAsync(BasicCommentOnSourceModel source);

	Task UpdateCommentAsync(string itemId, CommentModel comment);

	Task UpVoteCommentAsync(string itemId, string userId);

}
