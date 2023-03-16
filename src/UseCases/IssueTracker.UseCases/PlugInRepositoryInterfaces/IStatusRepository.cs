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
	Task CreateNewStatusAsync(StatusModel status);
	Task UpdateStatusAsync(StatusModel status);
	Task<StatusModel> ViewStatusByIdAsync(string statusId);
	Task<IEnumerable<StatusModel>> ViewStatusesAsync();
}