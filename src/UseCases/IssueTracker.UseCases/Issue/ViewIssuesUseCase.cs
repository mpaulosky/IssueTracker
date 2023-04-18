//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewIssuesUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class ViewIssuesUseCase : IViewIssuesUseCase
{

	private readonly IIssueRepository _issueRepository;

	public ViewIssuesUseCase(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task<IEnumerable<IssueModel>> ExecuteAsync()
	{

		return await _issueRepository.GetIssuesAsync();

	}

}
