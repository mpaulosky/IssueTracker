//-----------------------------------------------------------------------
// <copyright File="CreateNewUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users;

public class CreateNewUserUseCase : ICreateNewUserUseCase
{

	private readonly IUserRepository _userRepository;

	public CreateNewUserUseCase(IUserRepository userRepository)
	{

		_userRepository = userRepository;

	}

	public async Task ExecuteAsync(UserModel user)
	{

		if (user == null) return;

		await _userRepository.CreateNewUserAsync(user);

	}

}
