//-----------------------------------------------------------------------
// <copyright>
//	File:		UpdateIssueUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class UpdateIssueUseCase : IUpdateIssueUseCase
{

	private readonly IIssueRepository _issueRepository;

	public UpdateIssueUseCase(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task ExecuteAsync(IssueModel? issue)
	{

		if (issue == null)
		{

			return;

		}

		await _issueRepository.UpdateIssueAsync(issue);

	}

}
