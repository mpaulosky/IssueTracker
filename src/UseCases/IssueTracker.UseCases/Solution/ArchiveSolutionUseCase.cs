//-----------------------------------------------------------------------
// <copyright>
//	File:		ArchiveSolutionUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution;

public class ArchiveSolutionUseCase : IArchiveSolutionUseCase
{

	private readonly ISolutionRepository _solutionRepository;

	public ArchiveSolutionUseCase(ISolutionRepository solutionRepository)
	{

		_solutionRepository = solutionRepository;

	}

	public async Task ExecuteAsync(SolutionModel? solution)
	{

		ArgumentNullException.ThrowIfNull(solution);

		await _solutionRepository.ArchiveAsync(solution);

	}

}
