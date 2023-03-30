﻿//-----------------------------------------------------------------------
// <copyright File="IUserRepository"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.PlugInRepositoryInterfaces;

public interface IUserRepository
{

	Task CreateUserAsync(UserModel user);

	Task<UserModel> GetUserByIdAsync(string id);

	Task<UserModel> GetUserByAuthenticationIdAsync(string userObjectIdentifierId);

	Task<IEnumerable<UserModel>> GetUsersAsync();

	Task UpdateUserAsync(UserModel user);

}