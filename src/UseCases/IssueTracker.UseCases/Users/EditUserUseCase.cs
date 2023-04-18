//-----------------------------------------------------------------------
// <copyright File="EditUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users;

public class EditUserUseCase : IEditUserUseCase
{

	private readonly IUserRepository _userRepository;

	public EditUserUseCase(IUserRepository userRepository)
	{

		_userRepository = userRepository;

	}

	public async Task ExecuteAsync(UserModel user)
	{

		if (user == null) return;

		await _userRepository.UpdateUserAsync(user);

	}
}
