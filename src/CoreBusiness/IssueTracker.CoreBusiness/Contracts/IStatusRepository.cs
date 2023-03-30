//-----------------------------------------------------------------------
// <copyright file="IStatusRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Contracts;

public interface IStatusRepository
{

	Task ArchiveStatus(StatusModel status);

	Task CreateStatus(StatusModel status);

	Task<StatusModel> GetStatus(string itemId);

	Task<IEnumerable<StatusModel>> GetStatuses();

	Task UpdateStatus(string itemId, StatusModel status);

}