//-----------------------------------------------------------------------
// <copyright File="ViewStatusUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status.Interfaces;

public interface IViewStatusUseCase
{
	Task<StatusModel?> ExecuteAsync(string statusId);

}
