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

	Task ArchiveAsync(UserModel user);

	Task CreateAsync(UserModel user);

	Task<UserModel?> GetAsync(string id);

	Task<UserModel?> GetByAuthenticationIdAsync(string userObjectIdentifierId);

	Task<IEnumerable<UserModel>?> GetAllAsync(bool includeArchived = false);

	Task UpdateAsync(UserModel user);

}
