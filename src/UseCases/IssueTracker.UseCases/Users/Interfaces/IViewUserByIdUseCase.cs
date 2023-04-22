﻿//-----------------------------------------------------------------------
// <copyright File="IViewUserByIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users.Interfaces;

public interface IViewUserByIdUseCase
{
	Task<UserModel?> ExecuteAsync(string? id);
}
