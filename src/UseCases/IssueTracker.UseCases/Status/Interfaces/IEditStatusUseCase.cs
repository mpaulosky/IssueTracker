//-----------------------------------------------------------------------
// <copyright File="IEditStatusUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status.Interfaces;

public interface IEditStatusUseCase
{
	Task ExecuteAsync(StatusModel status);
}