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
	Task CreateStatusAsync(StatusModel status);
	Task UpdateStatusAsync(StatusModel status);
	Task<StatusModel> GetStatusByIdAsync(string statusId);
	Task<IEnumerable<StatusModel>> GetStatusesAsync();
}