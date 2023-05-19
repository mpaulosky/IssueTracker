//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewIssueUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class ViewIssueUseCase : IViewIssueUseCase
{

	private readonly IIssueRepository _issueRepository;

	public ViewIssueUseCase(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task<IssueModel?> ExecuteAsync(string? issueId)
	{

		ArgumentException.ThrowIfNullOrEmpty(issueId);

		return await _issueRepository.GetAsync(issueId);

	}

}
