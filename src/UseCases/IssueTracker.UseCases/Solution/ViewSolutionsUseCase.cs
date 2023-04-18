//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewSolutionsUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ViewSolutionsUseCase : IViewSolutionsUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public ViewSolutionsUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task<IEnumerable<SolutionModel>> ExecuteAsync()
	{

		return await _solutionRepository.GetSolutionsAsync();

	}
}
