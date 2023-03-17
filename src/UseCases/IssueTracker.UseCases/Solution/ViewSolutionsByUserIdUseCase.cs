//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewSolutionsByUserIdUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ViewSolutionsByUserIdUseCase : IViewSolutionsByUserIdUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public ViewSolutionsByUserIdUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task<IEnumerable<SolutionModel>> ExecuteAsync(UserModel user)
	{

		if (user == null) return new List<SolutionModel>();

		return await _solutionRepository.GetSolutionsByUserIdAsync(user.Id);

	}

}
