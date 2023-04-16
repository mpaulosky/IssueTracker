//-----------------------------------------------------------------------
// <copyright>
//	File:		EditIssueUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class EditIssueUseCase : IEditIssueUseCase
{

	private readonly IIssueRepository _issueRepository;

	public EditIssueUseCase(IIssueRepository issueRepository)
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
