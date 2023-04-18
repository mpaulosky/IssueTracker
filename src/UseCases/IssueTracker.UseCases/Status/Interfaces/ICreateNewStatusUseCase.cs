//-----------------------------------------------------------------------
// <copyright File="ICreateNewStatusUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status.Interfaces;

public interface ICreateNewStatusUseCase
{
	Task ExecuteAsync(StatusModel status);
}
