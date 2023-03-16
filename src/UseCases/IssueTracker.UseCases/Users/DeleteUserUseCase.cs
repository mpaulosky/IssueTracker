//-----------------------------------------------------------------------
// <copyright File="DeleteUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UseCases.PlugInRepositoryInterfaces;

namespace IssueTracker.UseCases.Users;

public class DeleteUserUseCase : IDeleteUserUseCase
{

	private readonly IUserRepository _userRepository;

	public DeleteUserUseCase(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task ExecuteAsync(UserModel user)
	{

		if (user == null) return;

		// Mark	user as in-active
		user.Archive = false;

		await _userRepository.UpdateUserAsync(user);

	}

}
