﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ISolutionRepository.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

public interface ISolutionRepository
{
	Task ArchiveAsync(SolutionModel solution);

	Task CreateAsync(SolutionModel solution);

	Task<SolutionModel> GetAsync(string solutionId);

	Task<IEnumerable<SolutionModel>> GetAllAsync();

	Task<IEnumerable<SolutionModel>> GetByUserAsync(string userId);

	Task<IEnumerable<SolutionModel>> GetByIssueAsync(string issueId);

	Task UpdateAsync(string itemId, SolutionModel solution);

	Task UpVoteAsync(string itemId, string userId);
}