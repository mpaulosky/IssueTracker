//-----------------------------------------------------------------------
// <copyright>
//	File:		UpdateSolutionUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class UpdateSolutionUseCase : IUpdateSolutionUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public UpdateSolutionUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task ExecuteAsync(SolutionModel solution)
	{

		Guard.Against.Null(solution, nameof(solution));

		await _solutionRepository.UpdateAsync(solution);

	}

}
