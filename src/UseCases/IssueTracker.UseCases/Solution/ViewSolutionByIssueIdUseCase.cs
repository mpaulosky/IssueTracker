//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewSolutionByIssueIdUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ViewSolutionsByIssueUseCase : IViewSolutionsByIssueUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public ViewSolutionsByIssueUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task<IEnumerable<SolutionModel>?> ExecuteAsync(BasicIssueModel? issue)
	{

		ArgumentNullException.ThrowIfNull(issue);

		return await _solutionRepository.GetByIssueAsync(issue);

	}

}
