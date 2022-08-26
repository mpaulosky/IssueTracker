//-----------------------------------------------------------------------
// <copyright file="IStatusService.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface IStatusService
{
	Task CreateStatus(StatusModel status);

	Task<StatusModel> GetStatus(string id);

	Task<List<StatusModel>> GetStatuses();

	Task UpdateStatus(StatusModel status);
}