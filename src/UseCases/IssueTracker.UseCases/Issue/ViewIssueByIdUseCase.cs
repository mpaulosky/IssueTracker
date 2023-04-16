//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewIssueByIdUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class ViewIssueByIdUseCase : IViewIssueByIdUseCase
{

	private readonly IIssueRepository _issueRepository;

	public ViewIssueByIdUseCase(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task<IssueModel?> ExecuteAsync(string? issueId)
	{
		if (string.IsNullOrWhiteSpace(issueId)) return null;

		return await _issueRepository.GetIssueByIdAsync(issueId);

	}

}
