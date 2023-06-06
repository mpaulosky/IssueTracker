//-----------------------------------------------------------------------
// <copyright>
//	File:		IUpVoteSolutionUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Solution.Interfaces;

public interface IUpVoteSolutionUseCase
{

	Task ExecuteAsync(SolutionModel? solution, UserModel user);

}
