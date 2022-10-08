//-----------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface IUserRepository
{
	Task<UserModel> GetUser(string itemId);

	Task<IEnumerable<UserModel>> GetUsers();

	Task CreateUser(UserModel user);

	Task UpdateUser(string itemId, UserModel user);

	Task<UserModel> GetUserFromAuthentication(string itemId);
}