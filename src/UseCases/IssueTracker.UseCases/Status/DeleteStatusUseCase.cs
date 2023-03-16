//-----------------------------------------------------------------------
// <copyright File="DeleteStatusUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UseCases.PlugInRepositoryInterfaces;

namespace IssueTracker.UseCases.Status;

public class DeleteStatusUseCase : IDeleteStatusUseCase
{

	private readonly IStatusRepository _statusRepository;

	public DeleteStatusUseCase(IStatusRepository statusRepository)
	{

		_statusRepository = statusRepository;

	}

	public async Task ExecuteAsync(StatusModel status)
	{

		if (status == null) return;

		// Mark as in-active
		status.Archive = false;

		await _statusRepository.UpdateStatusAsync(status);

	}

}
