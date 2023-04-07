//-----------------------------------------------------------------------
// <copyright file="IIssueRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.PlugInRepositoryInterfaces;

public interface IIssueRepository
{

	Task ArchiveIssueAsync(IssueModel issue);

	Task CreateIssueAsync(IssueModel issue);

	Task<IssueModel> GetIssueAsync(string itemId);

	Task<IEnumerable<IssueModel>> GetIssuesAsync();

	Task<IEnumerable<IssueModel>> GetApprovedIssuesAsync();

	Task<IEnumerable<IssueModel>> GetIssuesByUserAsync(string userId);

	Task<IEnumerable<IssueModel>> GetIssuesWaitingForApprovalAsync();

	Task UpdateIssueAsync(string itemId, IssueModel issue);

}