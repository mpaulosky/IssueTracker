//-----------------------------------------------------------------------
// <copyright>
//	File:		ArchiveSolutionUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ArchiveSolutionUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public ArchiveSolutionUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task ExecuteAsync(SolutionModel solution)
	{

		if (solution == null) return;

		// Archive the solution
		solution.Archived = true;

		await _solutionRepository.UpdateSolutionAsync(solution);

	}

}
