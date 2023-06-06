//-----------------------------------------------------------------------
// <copyright>
//	File:		ViewIssuesByUserId.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue;

public class ViewIssuesByUserUseCase : IViewIssuesByUserUseCase
{

	private readonly IIssueRepository _issueRepository;

	public ViewIssuesByUserUseCase(IIssueRepository issueRepository)
	{

		_issueRepository = issueRepository;

	}

	public async Task<IEnumerable<IssueModel>?> ExecuteAsync(UserModel? user)
	{

		ArgumentNullException.ThrowIfNull(user);

		return await _issueRepository.GetByUserAsync(user.Id);

	}

}
