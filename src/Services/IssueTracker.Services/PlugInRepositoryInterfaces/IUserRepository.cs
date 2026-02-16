// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IUserRepository.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

/// <summary>
/// Provides data access operations for user entities in the data store.
/// </summary>
public interface IUserRepository
{
	/// <summary>
	/// Archives a user by marking them as inactive in the data store.
	/// </summary>
	/// <param name="user">The user to archive.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
	Task ArchiveAsync(UserModel user);

	/// <summary>
	/// Creates a new user in the data store.
	/// </summary>
	/// <param name="user">The user to create.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
	Task CreateAsync(UserModel user);

	/// <summary>
	/// Retrieves a specific user from the data store by their unique identifier.
	/// </summary>
	/// <param name="itemId">The unique identifier of the user.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="UserModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="itemId"/> is null or empty.</exception>
	Task<UserModel> GetAsync(string itemId);

	/// <summary>
	/// Retrieves a user by their authentication provider object identifier.
	/// </summary>
	/// <param name="userObjectIdentifierId">The object identifier from the authentication provider.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="UserModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="userObjectIdentifierId"/> is null or empty.</exception>
	Task<UserModel> GetFromAuthenticationAsync(string userObjectIdentifierId);

	/// <summary>
	/// Retrieves all users from the data store.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains an enumerable collection of all <see cref="UserModel"/> instances.
	/// </returns>
	Task<IEnumerable<UserModel>> GetAllAsync();

	/// <summary>
	/// Updates an existing user in the data store.
	/// </summary>
	/// <param name="itemId">The unique identifier of the user to update.</param>
	/// <param name="user">The updated user data.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="itemId"/> is null or empty.</exception>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
	Task UpdateAsync(string itemId, UserModel user);
}
