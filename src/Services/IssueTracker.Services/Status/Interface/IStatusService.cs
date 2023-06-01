//-----------------------------------------------------------------------
// <copyright file="IStatusService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Services.Status.Interface;

public interface IStatusService
{
	Task CreateStatus(StatusModel status);

	Task<StatusModel> GetStatus(string statusId);

	Task<List<StatusModel>> GetStatuses();

	Task UpdateStatus(StatusModel status);

	Task DeleteStatus(StatusModel status);
}
