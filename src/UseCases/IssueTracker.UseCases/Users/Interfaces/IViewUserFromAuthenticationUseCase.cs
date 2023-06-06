//-----------------------------------------------------------------------
// <copyright File="IViewUserFromAuthenticationUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users.Interfaces;

public interface IViewUserFromAuthenticationUseCase
{

	Task<UserModel?> ExecuteAsync(string? userObjectIdentifierId);

}
