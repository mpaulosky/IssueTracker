//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewSolutionByIssueIdUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ViewSolutionByIssueIdUseCase : IViewSolutionByIssueIdUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public ViewSolutionByIssueIdUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task<SolutionModel> ExecuteAsync(IssueModel issue)
	{

		if (issue == null)
		{

			return new SolutionModel();

		}

		return await _solutionRepository.ViewSolutionByIssueIdAsync(issue.Id);

	}

}
