//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewIssuesByUserId.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class ViewIssuesByUserId : IViewIssuesByUserId
{

	private readonly IIssueRepository _issueRepository;

	public ViewIssuesByUserId(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task<IEnumerable<IssueModel>> ExecuteAsync(UserModel user)
	{

		if (user == null)
		{
			return new List<IssueModel>();
		}

		return await _issueRepository.GetIssuesByUserIdAsync(user.Id);

	}

}
