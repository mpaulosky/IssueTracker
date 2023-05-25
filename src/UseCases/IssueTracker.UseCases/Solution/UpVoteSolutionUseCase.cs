//-----------------------------------------------------------------------
// <copyright>
//	File:		UpVoteSolutionUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class UpVoteSolutionUseCase : IUpVoteSolutionUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public UpVoteSolutionUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task ExecuteAsync(SolutionModel? solution, UserModel? user)
	{

		ArgumentNullException.ThrowIfNull(solution);
		ArgumentNullException.ThrowIfNull(user);

		await _solutionRepository.UpVoteAsync(solution.Id, user.Id);

	}

}
