//-----------------------------------------------------------------------
// <copyright>
//	File:		CreateNewSolutionUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class CreateNewSolutionUseCase : ICreateNewSolutionUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public CreateNewSolutionUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task ExecuteAsync(SolutionModel solution)
	{

		if (solution == null) return;

		await _solutionRepository.CreateSolutionAsync(solution);

	}

}
