﻿//-----------------------------------------------------------------------
// <copyright>
//	File:		IViewSolutionsByUserUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution.Interfaces;

public interface IViewSolutionsByUserUseCase
{
	Task<IEnumerable<SolutionModel>?> ExecuteAsync(UserModel user);

}
