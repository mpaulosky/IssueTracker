//-----------------------------------------------------------------------
// <copyright File="IStatusRepository"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.PlugInRepositoryInterfaces;

public interface IStatusRepository
{

	Task ArchiveAsync(StatusModel status);

	Task CreateAsync(StatusModel status);

	Task<StatusModel?> GetAsync(string statusId);

	Task<IEnumerable<StatusModel>?> GetAllAsync(bool includeArchived = false);

	Task UpdateAsync(StatusModel status);

}
