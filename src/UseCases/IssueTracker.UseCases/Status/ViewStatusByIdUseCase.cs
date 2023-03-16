//-----------------------------------------------------------------------
// <copyright File="ViewStatusByIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status;

public class ViewStatusByIdUseCase : IViewStatusByIdUseCase
{

	private readonly IStatusRepository _statusRepository;

	public ViewStatusByIdUseCase(IStatusRepository statusRepository)
	{

		_statusRepository = statusRepository;

	}

	public async Task<StatusModel> ExecuteAsync(string statusId)
	{

		if (string.IsNullOrWhiteSpace(statusId)) return new();

		return await _statusRepository.ViewStatusByIdAsync(statusId);

	}

}
