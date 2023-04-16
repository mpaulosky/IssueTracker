﻿//-----------------------------------------------------------------------
// <copyright file="IUserService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Services.Interfaces;

public interface IUserService
{
	Task CreateUser(UserModel user);

	Task<UserModel> GetUser(string? userId);

	Task<UserModel> GetUserFromAuthentication(string? userObjectIdentifierId);

	Task<List<UserModel>> GetUsers();

	Task UpdateUser(UserModel user);
}
