//-----------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.PlugInRepositoryInterfaces;

public interface IUserRepository
{

	Task ArchiveUserAsync(UserModel user);

	Task CreateUserAsync(UserModel user);

	Task<UserModel> GetUserAsync(string itemId);

	Task<UserModel> GetUserFromAuthenticationAsync(string userObjectIdentifierId);

	Task<IEnumerable<UserModel>> GetUsersAsync();

	Task UpdateUserAsync(string itemId, UserModel user);

}
