//-----------------------------------------------------------------------
// <copyright>
//	File:		IEditIssueUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Issue.Interfaces;

public interface IEditIssueUseCase
{
	Task ExecuteAsync(IssueModel issue);
}