//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewSolutionByIdUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ViewSolutionByIdUseCase : IViewSolutionByIdUseCase
{


	private readonly ISolutionRepository _solutionRepository;

	public ViewSolutionByIdUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task<SolutionModel?> ExecuteAsync(string? solutionId)
	{

		if (string.IsNullOrWhiteSpace(solutionId)) return null;

		return await _solutionRepository.GetSolution(solutionId);

	}
}
