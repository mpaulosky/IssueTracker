//-----------------------------------------------------------------------
// <copyright>
//	File:		ISolutionRepository.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.PlugInRepositoryInterfaces;

public interface ISolutionRepository
{
	Task CreateNewSolutionAsync(SolutionModel solution);
}
