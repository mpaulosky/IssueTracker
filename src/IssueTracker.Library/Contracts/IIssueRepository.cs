//-----------------------------------------------------------------------
// <copyright file="IIssueRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface IIssueRepository
{

	Task CreateIssue(IssueModel issue);

	Task<IssueModel> GetIssue(string itemId);

	Task<IEnumerable<IssueModel>> GetIssues();

	Task<IEnumerable<IssueModel>> GetApprovedIssues();

	Task<IEnumerable<IssueModel>> GetIssuesByUser(string userId);

	Task<IEnumerable<IssueModel>> GetIssuesWaitingForApproval();

	Task UpdateIssue(string itemId, IssueModel issue);

}