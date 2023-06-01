//-----------------------------------------------------------------------
// <copyright file="ICommentRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

public interface ICommentRepository
{
	Task ArchiveAsync(CommentModel comment);

	Task CreateAsync(CommentModel comment);

	Task<CommentModel> GetAsync(string itemId);

	Task<IEnumerable<CommentModel>?> GetAllAsync();

	Task<IEnumerable<CommentModel>> GetByUserAsync(string userId);

	Task<IEnumerable<CommentModel>> GetBySourceAsync(BasicCommentOnSourceModel source);

	Task UpdateAsync(string itemId, CommentModel comment);

	Task UpVoteAsync(string itemId, string userId);
}
