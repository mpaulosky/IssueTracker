//-----------------------------------------------------------------------
// <copyright File="UpdateStatusUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status;

public class UpdateStatusUseCase : IUpdateStatusUseCase
{

	private readonly IStatusRepository _statusRepository;

	public UpdateStatusUseCase(IStatusRepository statusRepository)
	{

		_statusRepository = statusRepository;

	}

	public async Task ExecuteAsync(StatusModel status)
	{

		Guard.Against.Null(status, nameof(status));

		await _statusRepository.UpdateAsync(status);

	}

}
