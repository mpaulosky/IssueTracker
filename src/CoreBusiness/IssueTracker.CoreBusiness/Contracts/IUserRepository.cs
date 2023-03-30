//-----------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Contracts;

public interface IUserRepository
{

	Task ArchiveUser(UserModel user);

	Task CreateUser(UserModel user);

	Task<UserModel> GetUser(string itemId);

	Task<UserModel> GetUserFromAuthentication(string userObjectIdentifierId);

	Task<IEnumerable<UserModel>> GetUsers();

	Task UpdateUser(string itemId, UserModel user);

}