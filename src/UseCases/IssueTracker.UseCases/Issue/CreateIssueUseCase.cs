//-----------------------------------------------------------------------
// <copyright>
//	File:		CreateIssueUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class CreateIssueUseCase : ICreateIssueUseCase
{

	private readonly IIssueRepository _issueRepository;

	public CreateIssueUseCase(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task ExecuteAsync(IssueModel issue)
	{

		Guard.Against.Null(issue, nameof(issue));

		await _issueRepository.CreateAsync(issue);

	}
}
