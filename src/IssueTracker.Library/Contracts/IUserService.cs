//-----------------------------------------------------------------------
// <copyright file="IUserService.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface IUserService
{
	Task CreateUser(UserModel user);

	Task<UserModel> GetUser(string userId);

	Task<UserModel> GetUserFromAuthentication(string userId);

	Task<List<UserModel>> GetUsers();

	Task UpdateUser(UserModel user);
}