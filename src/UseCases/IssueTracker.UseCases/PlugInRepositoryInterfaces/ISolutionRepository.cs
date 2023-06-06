﻿//-----------------------------------------------------------------------
// <copyright>
//	File:		ISolutionRepository.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.PlugInRepositoryInterfaces;

public interface ISolutionRepository
{

	Task ArchiveAsync(SolutionModel solution);

	Task CreateAsync(SolutionModel? solution);

	Task<SolutionModel?> GetAsync(string? solutionId);

	Task<IEnumerable<SolutionModel>?> GetAllAsync(bool includeArchived = false);

	Task<IEnumerable<SolutionModel>?> GetByIssueAsync(BasicIssueModel? issue);

	Task<IEnumerable<SolutionModel>?> GetByUserAsync(BasicUserModel user);

	Task UpdateAsync(SolutionModel? solution);

	Task UpVoteAsync(string? solutionId, string? userId);

}