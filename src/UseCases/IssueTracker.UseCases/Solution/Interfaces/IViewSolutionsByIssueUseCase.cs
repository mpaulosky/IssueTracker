//-----------------------------------------------------------------------
// <copyright>
//	File:		IViewSolutionByIssueUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution.Interfaces;

public interface IViewSolutionsByIssueUseCase
{

	Task<IEnumerable<SolutionModel>?> ExecuteAsync(BasicIssueModel? issue);

}
