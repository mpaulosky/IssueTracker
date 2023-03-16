//-----------------------------------------------------------------------
// <copyright>
//	File:		IViewSolutionsByUserIdUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution.Interfaces;

public interface IViewSolutionsByUserIdUseCase
{
	Task<IEnumerable<SolutionModel>> ExecuteAsync(UserModel user);
}