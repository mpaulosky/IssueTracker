//-----------------------------------------------------------------------
// <copyright File="ViewUsersUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users;

public class ViewUsersUseCase : IViewUsersUseCase
{

	private readonly IUserRepository _userRepository;

	public ViewUsersUseCase(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<IEnumerable<UserModel>> ExecuteAsync()
	{

		return await _userRepository.GetUsersAsync();

	}

}
