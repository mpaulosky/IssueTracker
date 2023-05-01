//-----------------------------------------------------------------------
// <copyright File="ViewUserUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users;

public class ViewUserUseCase : IViewUserUseCase
{

	private readonly IUserRepository _userRepository;

	public ViewUserUseCase(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<UserModel?> ExecuteAsync(string id)
	{

		Guard.Against.NullOrWhiteSpace(id, nameof(id));

		return await _userRepository.GetAsync(id);

	}

}
