//-----------------------------------------------------------------------
// <copyright>
//	File:		IEditSolutionUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution.Interfaces;

public interface IEditSolutionUseCase
{
	Task ExecuteAsync(SolutionModel solution);
}
