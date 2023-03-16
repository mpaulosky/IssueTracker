//-----------------------------------------------------------------------
// <copyright File="CreateNewStatusUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status;

public class CreateNewStatusUseCase : ICreateNewStatusUseCase
{

	private readonly IStatusRepository _statusRepository;

	public CreateNewStatusUseCase(IStatusRepository statusRepository)
	{

		_statusRepository = statusRepository;

	}

	public async Task ExecuteAsync(StatusModel status)
	{

		if (status == null) return;

		await _statusRepository.CreateNewStatusAsync(status);

	}

}
