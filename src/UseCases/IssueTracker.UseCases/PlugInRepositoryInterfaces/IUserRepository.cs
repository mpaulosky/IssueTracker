//-----------------------------------------------------------------------
// <copyright File="IUserRepository"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.PlugInRepositoryInterfaces;

public interface IUserRepository
{
	Task CreateNewUserAsync(UserModel user);
	Task UpdateUserAsync(UserModel user);
	Task<IEnumerable<UserModel>> ViewUsersAsync();
	Task<UserModel> ViewUserByAuthenticationIdAsync(string userObjectIdentifierId);
	Task<UserModel> ViewUserByIdAsync(string id);
}