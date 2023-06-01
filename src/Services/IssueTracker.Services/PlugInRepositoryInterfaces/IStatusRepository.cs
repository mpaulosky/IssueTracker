//-----------------------------------------------------------------------
// <copyright file="IStatusRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

public interface IStatusRepository
{
	Task ArchiveAsync(StatusModel status);

	Task CreateAsync(StatusModel status);

	Task<StatusModel> GetAsync(string itemId);

	Task<IEnumerable<StatusModel>> GetAllAsync();

	Task UpdateAsync(string itemId, StatusModel status);
}
