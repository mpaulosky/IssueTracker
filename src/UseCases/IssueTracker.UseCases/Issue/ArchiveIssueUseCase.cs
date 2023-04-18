//-----------------------------------------------------------------------
// <copyright>
//	File:		ArchiveIssueUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class ArchiveIssueUseCase : IArchiveIssueUseCase
{

	private readonly IIssueRepository _issueRepository;

	public ArchiveIssueUseCase(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task ExecuteAsync(IssueModel? issue)
	{

		if (issue == null)
		{
			return;
		}

		// Archive the issue
		issue.Archived = true;

		await _issueRepository.UpdateIssueAsync(issue);

	}

}
