//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewSolutionByIssueIdUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ViewSolutionsByIssueIdUseCase : IViewSolutionsByIssueIdUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public ViewSolutionsByIssueIdUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task<IEnumerable<SolutionModel>?> ExecuteAsync(BasicIssueModel issue)
	{

		Guard.Against.Null(issue, nameof(issue));

		return await _solutionRepository.GetByIssueAsync(issue.Id);

	}

}
