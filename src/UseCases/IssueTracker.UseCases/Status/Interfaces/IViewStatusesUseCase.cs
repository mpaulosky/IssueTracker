//-----------------------------------------------------------------------
// <copyright File="IViewStatusesUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Status.Interfaces;

public interface IViewStatusesUseCase
{
	Task<IEnumerable<StatusModel>?> ExecuteAsync(bool includeArchived = false);

}
