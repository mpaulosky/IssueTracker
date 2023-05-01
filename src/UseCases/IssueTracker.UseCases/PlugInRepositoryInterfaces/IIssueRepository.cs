//-----------------------------------------------------------------------
// <copyright File="IIssueRepository"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.PlugInRepositoryInterfaces;

public interface IIssueRepository
{

	Task ArchiveAsync(IssueModel issue);

	Task CreateAsync(IssueModel issue);

	Task<IssueModel?> GetAsync(string id);

	Task<IEnumerable<IssueModel>?> GetAllAsync(bool includeArchived = false);

	Task<IEnumerable<IssueModel>?> GetApprovedAsync();

	Task<IEnumerable<IssueModel>?> GetByUserAsync(string id);

	Task<IEnumerable<IssueModel>?> GetWaitingForApprovalAsync();

	Task UpdateIssueAsync(IssueModel issue);

}
