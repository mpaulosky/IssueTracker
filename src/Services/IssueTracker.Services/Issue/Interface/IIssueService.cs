//-----------------------------------------------------------------------
// <copyright file="IIssueService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Services.Issue.Interface;

public interface IIssueService
{
	Task CreateIssue(IssueModel issue);

	Task<IssueModel> GetIssue(string? issueId);

	Task<List<IssueModel>> GetIssues();

	Task<List<IssueModel>> GetIssuesByUser(string userId);

	Task<List<IssueModel>> GetApprovedIssues();

	Task<List<IssueModel>> GetIssuesWaitingForApproval();

	Task UpdateIssue(IssueModel issue);
}
