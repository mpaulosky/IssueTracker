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
	Task CreateNewSolutionAsync(SolutionModel solution);
	Task UpdateSolutionAsync(SolutionModel solution);
	Task<SolutionModel> ViewSolutionByIssueIdAsync(string id);
	Task<IEnumerable<SolutionModel>> ViewSolutionsAsync();
	Task<IEnumerable<SolutionModel>> ViewSolutionsByUserIdAsync(string id);
}
