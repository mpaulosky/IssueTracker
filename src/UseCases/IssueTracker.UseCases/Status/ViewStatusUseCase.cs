//-----------------------------------------------------------------------
// <copyright File="ViewStatusUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status;

public class ViewStatusUseCase : IViewStatusUseCase
{

	private readonly IStatusRepository _statusRepository;

	public ViewStatusUseCase(IStatusRepository statusRepository)
	{

		_statusRepository = statusRepository;

	}

	public async Task<StatusModel?> ExecuteAsync(string statusId)
	{

		Guard.Against.NullOrWhiteSpace(statusId, nameof(statusId));

		return await _statusRepository.GetAsync(statusId);

	}

}
