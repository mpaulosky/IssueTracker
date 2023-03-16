//-----------------------------------------------------------------------
// <copyright File="EditStatusUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status;

public class EditStatusUseCase : IEditStatusUseCase
{

	private readonly IStatusRepository _statusRepository;

	public EditStatusUseCase(IStatusRepository statusRepository)
	{

		_statusRepository = statusRepository;

	}

	public async Task ExecuteAsync(StatusModel status)
	{

		if (status == null) return;

		await _statusRepository.UpdateStatusAsync(status);

	}

}
