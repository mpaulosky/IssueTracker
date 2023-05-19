//-----------------------------------------------------------------------
// <copyright File="IUpdateUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users.Interfaces;

public interface IUpdateUserUseCase
{

	Task ExecuteAsync(UserModel? user);

}
