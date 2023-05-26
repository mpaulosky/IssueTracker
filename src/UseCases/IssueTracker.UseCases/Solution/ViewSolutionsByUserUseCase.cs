//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewSolutionsByUserUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ViewSolutionsByUserUseCase : IViewSolutionsByUserUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public ViewSolutionsByUserUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task<IEnumerable<SolutionModel>?> ExecuteAsync(BasicUserModel? user)
	{

		ArgumentNullException.ThrowIfNull(user);

		return await _solutionRepository.GetByUserAsync(user);

	}

}
