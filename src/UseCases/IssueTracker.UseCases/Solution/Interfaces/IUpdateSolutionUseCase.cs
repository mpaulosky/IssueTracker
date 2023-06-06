//-----------------------------------------------------------------------
// <copyright>
//	File:		IUpdateSolutionUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution.Interfaces;

public interface IUpdateSolutionUseCase
{
	Task ExecuteAsync(SolutionModel solution);

}
