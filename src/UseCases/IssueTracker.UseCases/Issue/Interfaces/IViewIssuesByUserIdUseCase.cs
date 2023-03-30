//-----------------------------------------------------------------------
// <copyright>
//	File:		IViewIssuesByUserId.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue.Interfaces;

public interface IViewIssuesByUserIdUseCase
{

	Task<IEnumerable<IssueModel>> ExecuteAsync(UserModel user);

}