//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewIssuesApprovedUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class ViewIssuesApprovedUseCase : IViewIssuesApprovedUseCase
{

	private readonly IIssueRepository _issueRepository;

	public ViewIssuesApprovedUseCase(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task<IEnumerable<IssueModel>> ExecuteAsync()
	{

		return await _issueRepository.GetIssuesApprovedAsync();

	}

}
