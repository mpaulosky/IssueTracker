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
	Task CreateIssueAsync(IssueModel issue);
	Task UpdateIssueAsync(IssueModel issue);
	Task<IEnumerable<IssueModel>> GetIssuesApprovedAsync();
	Task<IEnumerable<IssueModel>> GetIssuesAsync();
	Task<IEnumerable<IssueModel>> GetIssuesByUserIdAsync(string id);
	Task<IEnumerable<IssueModel>> GetIssuesWaitingForApprovalAsync();
}
