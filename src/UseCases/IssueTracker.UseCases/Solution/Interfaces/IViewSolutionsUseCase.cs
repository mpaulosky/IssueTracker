//-----------------------------------------------------------------------
// <copyright>
//	File:		IViewSolutionsUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution.Interfaces;

public interface IViewSolutionsUseCase
{
	Task<IEnumerable<SolutionModel>> ExecuteAsync(bool includeArchived = false);
	
}
