//-----------------------------------------------------------------------
// <copyright>
//	File:		IUpdateIssueUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue.Interfaces;

public interface IUpdateIssueUseCase
{
	Task ExecuteAsync(IssueModel issue);
	
}
