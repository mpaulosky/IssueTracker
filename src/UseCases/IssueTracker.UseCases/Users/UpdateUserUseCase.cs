//-----------------------------------------------------------------------
// <copyright File="UpdateUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users;

public class UpdateUserUseCase : IUpdateUserUseCase
{

	private readonly IUserRepository _userRepository;

	public UpdateUserUseCase(IUserRepository userRepository)
	{

		_userRepository = userRepository;

	}

	public async Task ExecuteAsync(UserModel? user)
	{

		ArgumentNullException.ThrowIfNull(user);

		await _userRepository.UpdateAsync(user);

	}
}
