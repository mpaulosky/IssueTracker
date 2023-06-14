// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ICommentRepository.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

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