﻿// Copyright (c) 2023. All rights reserved.
// File Name :     UserService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services

namespace IssueTracker.Services.User;

/// <summary>
///   UserService class
/// </summary>
public class UserService : IUserService
{
	private readonly IUserRepository _repo;

	/// <summary>
	///   UserService constructor
	/// </summary>
	/// <param name="repository">IUserRepository</param>
	/// <exception cref="ArgumentNullException"></exception>
	public UserService(IUserRepository repository)
	{
		ArgumentNullException.ThrowIfNull(repository);
		_repo = repository;
	}

	/// <summary>
	///   CreateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateUser(UserModel user)
	{
		ArgumentNullException.ThrowIfNull(user);

		return _repo.CreateAsync(user);
	}

	/// <summary>
	///   GetUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of UserModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<UserModel> GetUser(string? userId)
	{
		ArgumentException.ThrowIfNullOrEmpty(userId);

		UserModel results = await _repo.GetAsync(userId);

		return results;
	}

	/// <summary>
	///   GetUsers method
	/// </summary>
	/// <returns>Task if List UserModel</returns>
	public async Task<List<UserModel>> GetUsers()
	{
		IEnumerable<UserModel> results = await _repo.GetAllAsync();

		return results.ToList();
	}

	/// <summary>
	///   GetUserFromAuthentication method
	/// </summary>
	/// <param name="userObjectIdentifierId">string</param>
	/// <returns>Task of UserModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<UserModel> GetUserFromAuthentication(string? userObjectIdentifierId)
	{
		ArgumentException.ThrowIfNullOrEmpty(userObjectIdentifierId);

		UserModel results = await _repo.GetFromAuthenticationAsync(userObjectIdentifierId);

		return results;
	}

	/// <summary>
	///   UpdateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateUser(UserModel user)
	{
		ArgumentNullException.ThrowIfNull(user);

		return _repo.UpdateAsync(user.Id, user);
	}
}