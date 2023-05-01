//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewSolutionUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ViewSolutionUseCase : IViewSolutionUseCase
{


	private readonly ISolutionRepository _solutionRepository;

	public ViewSolutionUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task<SolutionModel?> ExecuteAsync(string solutionId)
	{

		Guard.Against.NullOrWhiteSpace(solutionId, nameof(solutionId));

		return await _solutionRepository.GetAsync(solutionId);

	}
}
