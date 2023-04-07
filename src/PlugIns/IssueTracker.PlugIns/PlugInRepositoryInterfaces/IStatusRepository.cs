//-----------------------------------------------------------------------
// <copyright file="IStatusRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.PlugInRepositoryInterfaces;

public interface IStatusRepository
{

	Task ArchiveStatusAsync(StatusModel status);

	Task CreateStatusAsync(StatusModel status);

	Task<StatusModel> GetStatusAsync(string itemId);

	Task<IEnumerable<StatusModel>> GetStatusesAsync();

	Task UpdateStatusAsync(string itemId, StatusModel status);

}
