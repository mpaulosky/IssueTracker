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
	Task CreateNewIssueAsync(IssueModel issue);
	Task EditIssueAsync(IssueModel issue);
	Task UpdateIssueAsync(IssueModel issue);
	Task<IEnumerable<IssueModel>> ViewIssuesApprovedAsync();
	Task<IEnumerable<IssueModel>> ViewIssuesAsync();
	Task<IEnumerable<IssueModel>> ViewIssuesByUserIdAsync(string id);
	Task<IEnumerable<IssueModel>> ViewIssuesWaitingForApprovalAsync();
}
