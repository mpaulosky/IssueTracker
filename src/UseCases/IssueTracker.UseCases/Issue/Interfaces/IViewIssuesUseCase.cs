//-----------------------------------------------------------------------
// <copyright>
//	File:		IViewIssuesUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue.Interfaces;

public interface IViewIssuesUseCase
{
	Task<IEnumerable<IssueModel>?> ExecuteAsync(bool includeArchived = false);

}
