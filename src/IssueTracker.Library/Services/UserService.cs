//-----------------------------------------------------------------------
// <copyright file="UserService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Services;

/// <summary>
///		UserService class
/// </summary>
public class UserService : IUserService
{
	private readonly IUserRepository _repo;

	/// <summary>
	///		UserService constructor
	/// </summary>
	/// <param name="repository">IUserRepository</param>
	/// <exception cref="ArgumentNullException"></exception>
	public UserService(IUserRepository repository)
	{
		_repo = Guard.Against.Null(repository, nameof(repository));
	}

	/// <summary>
	///		CreateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateUser(UserModel user)
	{
		Guard.Against.Null(user, nameof(user));

		return _repo.CreateUser(user);
	}

	/// <summary>
	///		GetUser method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of UserModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<UserModel> GetUser(string userId)
	{
		Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

		UserModel results = await _repo.GetUser(userId);

		return results;
	}

	/// <summary>
	///		GetUsers method
	/// </summary>
	/// <returns>Task if List UserModel</returns>
	public async Task<List<UserModel>> GetUsers()
	{
		IEnumerable<UserModel> results = await _repo.GetUsers();

		return results.ToList();
	}

	/// <summary>
	///		GetUserFromAuthentication method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of UserModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<UserModel> GetUserFromAuthentication(string userId)
	{
		Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

		UserModel results = await _repo.GetUserFromAuthentication(userId);

		return results;
	}

	/// <summary>
	///		UpdateUser method
	/// </summary>
	/// <param name="user">UserModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateUser(UserModel user)
	{
		Guard.Against.Null(user, nameof(user));

		return _repo.UpdateUser(user.Id, user);
	}
}