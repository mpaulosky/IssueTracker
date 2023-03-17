//-----------------------------------------------------------------------
// <copyright File="ViewStatusesUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status;

public class ViewStatusesUseCase : IViewStatusesUseCase
{

	private readonly IStatusRepository _statusRepository;

	public ViewStatusesUseCase(IStatusRepository statusRepository)
	{

		_statusRepository = statusRepository;

	}

	public async Task<IEnumerable<StatusModel>> ExecuteAsync()
	{

		return await _statusRepository.GetStatusesAsync();

	}

}
