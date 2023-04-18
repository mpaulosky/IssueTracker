//-----------------------------------------------------------------------
// <copyright>
//	File:		ArchiveStatusUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status;

public class ArchiveStatusUseCase : IArchiveStatusUseCase
{

	private readonly IStatusRepository _statusRepository;

	public ArchiveStatusUseCase(IStatusRepository statusRepository)
	{

		_statusRepository = statusRepository;

	}

	public async Task ExecuteAsync(StatusModel status)
	{

		if (status == null) return;

		// Archive the status
		status.Archived = true;

		await _statusRepository.UpdateStatusAsync(status);

	}

}
