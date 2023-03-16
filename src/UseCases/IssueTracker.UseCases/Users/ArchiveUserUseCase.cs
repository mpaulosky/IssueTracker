//-----------------------------------------------------------------------
// <copyright File="ArchiveUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users;

public class ArchiveUserUseCase : IArchiveUserUseCase
{

	private readonly IUserRepository _userRepository;

	public ArchiveUserUseCase(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task ExecuteAsync(UserModel user)
	{

		if (user == null) return;

		// Archive the user
		user.Archived = true;

		await _userRepository.UpdateUserAsync(user);

	}

}
