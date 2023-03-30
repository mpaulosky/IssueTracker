//-----------------------------------------------------------------------
// <copyright>
//	File:		CreateNewIssueUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class CreateNewIssueUseCase : ICreateNewIssueUseCase
{

	private readonly IIssueRepository _issueRepository;

	public CreateNewIssueUseCase(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task ExecuteAsync(IssueModel issue)
	{

		if (issue == null)
		{
			return;
		}

		await _issueRepository.CreateIssueAsync(issue);

	}
}

