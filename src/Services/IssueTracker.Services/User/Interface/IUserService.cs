// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IUserService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.User.Interface;

/// <summary>
/// Provides operations for managing users in the system.
/// </summary>
public interface IUserService
{
	/// <summary>
	/// Archives an existing user, marking them as inactive.
	/// </summary>
	/// <param name="user">The user to archive.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
	Task ArchiveUser(UserModel user);

	/// <summary>
	/// Creates a new user in the system.
	/// </summary>
	/// <param name="user">The user to create.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
	Task CreateUser(UserModel user);

	/// <summary>
	/// Retrieves a specific user by their unique identifier.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="UserModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> is null or empty.</exception>
	Task<UserModel> GetUser(string? userId);

	/// <summary>
	/// Retrieves a user by their authentication provider object identifier.
	/// </summary>
	/// <param name="userObjectIdentifierId">The object identifier from the authentication provider.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="UserModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="userObjectIdentifierId"/> is null or empty.</exception>
	Task<UserModel> GetUserFromAuthentication(string? userObjectIdentifierId);

	/// <summary>
	/// Retrieves all users from the system with caching support.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of all <see cref="UserModel"/> instances.
	/// </returns>
	/// <remarks>
	/// This method uses in-memory caching to improve performance.
	/// </remarks>
	Task<List<UserModel>> GetUsers();

	/// <summary>
	/// Updates an existing user with new information.
	/// </summary>
	/// <param name="user">The user with updated information.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
	Task UpdateUser(UserModel user);
}
