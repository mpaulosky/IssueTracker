// Copyright (c) 2023. All rights reserved.
// File Name :     ISolutionService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services

namespace IssueTracker.Services.Solution.Interface;

public interface ISolutionService
{
	Task ArchiveSolution(SolutionModel solution);

	Task CreateSolution(SolutionModel solution);

	Task<SolutionModel> GetSolution(string? solutionId);

	Task<List<SolutionModel>> GetSolutions();

	Task<List<SolutionModel>> GetSolutionsByUser(string userId);

	Task<List<SolutionModel>> GetSolutionsByIssue(string issueId);

	Task UpdateSolution(SolutionModel solution);

	Task UpVoteSolution(string solutionId, string userId);
}