//-----------------------------------------------------------------------
// <copyright file="IIssueService.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.Library.Contracts;

public interface IIssueService
{
	Task CreateIssue(IssueModel suggestion);

	Task<IssueModel> GetIssue(string id);

	Task<List<IssueModel>> GetIssues();

	Task<List<IssueModel>> GetUsersIssues(string userId);

	Task UpdateIssue(IssueModel suggestion);

	Task<List<IssueModel>> GetIssuesWaitingForApproval();

	Task<List<IssueModel>> GetApprovedIssues();
}