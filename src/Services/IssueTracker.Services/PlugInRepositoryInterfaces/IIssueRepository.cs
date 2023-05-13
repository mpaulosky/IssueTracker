﻿//-----------------------------------------------------------------------
// <copyright file="IIssueRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

public interface IIssueRepository
{

	Task ArchiveAsync(IssueModel issue);

	Task CreateAsync(IssueModel issue);

	Task<IssueModel> GetAsync(string itemId);

	Task<IEnumerable<IssueModel>> GetAllAsync();

	Task<IEnumerable<IssueModel>> GetApprovedAsync();

	Task<IEnumerable<IssueModel>> GetByUserAsync(string userId);

	Task<IEnumerable<IssueModel>> GetWaitingForApprovalAsync();

	Task UpdateAsync(string itemId, IssueModel issue);

}
