﻿//-----------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

public interface IUserRepository
{

	Task ArchiveAsync(UserModel user);

	Task CreateAsync(UserModel user);

	Task<UserModel> GetAsync(string itemId);

	Task<UserModel> GetFromAuthenticationAsync(string userObjectIdentifierId);

	Task<IEnumerable<UserModel>> GetAllAsync();

	Task UpdateAsync(string itemId, UserModel user);

}
