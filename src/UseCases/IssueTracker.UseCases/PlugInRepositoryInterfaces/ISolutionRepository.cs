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

	Task<SolutionModel?> GetSolution(string solutionId);

	Task<IEnumerable<SolutionModel>> GetSolutionsAsync();

	Task<IEnumerable<SolutionModel>> GetSolutionsByIssueIdAsync(string id);

	Task<IEnumerable<SolutionModel>> GetSolutionsByUserIdAsync(string id);

	Task UpdateSolutionAsync(SolutionModel solution);

}
