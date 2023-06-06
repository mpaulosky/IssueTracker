//-----------------------------------------------------------------------
// <copyright File="IViewUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users.Interfaces;

public interface IViewUserUseCase
{

	Task<UserModel?> ExecuteAsync(string? userId);

}
