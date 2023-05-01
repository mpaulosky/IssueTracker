//-----------------------------------------------------------------------
// <copyright File="CreateUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users;

public class CreateUserUseCase : ICreateUserUseCase
{

	private readonly IUserRepository _userRepository;

	public CreateUserUseCase(IUserRepository userRepository)
	{

		_userRepository = userRepository;

	}

	public async Task ExecuteAsync(UserModel user)
	{

		Guard.Against.Null(user, nameof(user));

		await _userRepository.CreateAsync(user);

	}

}
