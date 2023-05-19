//-----------------------------------------------------------------------
// <copyright File="ICommentRepository"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.PlugInRepositoryInterfaces;

public interface ICommentRepository
{

	Task ArchiveAsync(CommentModel comment);

	Task CreateAsync(CommentModel comment);

	Task<CommentModel?> GetAsync(string commentId);

	Task<IEnumerable<CommentModel>?> GetAllAsync(bool includeArchived = false);

	Task<IEnumerable<CommentModel>?> GetBySourceAsync(BasicCommentOnSourceModel source);

	Task<IEnumerable<CommentModel>?> GetByUserAsync(string userId);

	Task UpdateAsync(CommentModel comment);

	Task UpVoteAsync(string? commentId, string? userId);

}
