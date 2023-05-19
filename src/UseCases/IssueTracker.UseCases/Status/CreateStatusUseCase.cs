//-----------------------------------------------------------------------
// <copyright File="CreateStatusUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status;

public class CreateStatusUseCase : ICreateStatusUseCase
{

	private readonly IStatusRepository _statusRepository;

	public CreateStatusUseCase(IStatusRepository statusRepository)
	{

		_statusRepository = statusRepository;

	}

	public async Task ExecuteAsync(StatusModel? status)
	{

		ArgumentNullException.ThrowIfNull(status);

		await _statusRepository.CreateAsync(status);

	}

}
