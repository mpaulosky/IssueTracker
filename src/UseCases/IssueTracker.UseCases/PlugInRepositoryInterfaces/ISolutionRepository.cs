//-----------------------------------------------------------------------
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
	Task CreateSolutionAsync(SolutionModel solution);
	Task UpdateSolutionAsync(SolutionModel solution);
	Task<SolutionModel> GetSolutionByIssueIdAsync(string id);
	Task<IEnumerable<SolutionModel>> GetSolutionsAsync();
	Task<IEnumerable<SolutionModel>> GetSolutionsByUserIdAsync(string id);
}
