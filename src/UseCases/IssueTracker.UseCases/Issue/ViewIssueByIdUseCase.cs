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

	public async Task<IssueModel> ExecuteAsync(string id)
	{

		return await _issueRepository.GetIssueByIdAsync(id);

	}

}
