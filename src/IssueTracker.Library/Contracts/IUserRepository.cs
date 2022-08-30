//-----------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface IUserRepository
{
	Task<UserModel> GetUser(string userId);

	Task<IEnumerable<UserModel>> GetUsers();

	Task CreateUser(UserModel user);

	Task UpdateUser(string id, UserModel user);

	Task<UserModel> GetUserFromAuthentication(string userId);
}