//-----------------------------------------------------------------------
// <copyright File="ViewUserByIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UseCases.PlugInRepositoryInterfaces;

namespace IssueTracker.UseCases.Users;

public class ViewUserByIdUseCase : IViewUserByIdUseCase
{

	private readonly IUserRepository _userRepository;

	public ViewUserByIdUseCase(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<UserModel> ExecuteAsync(string id)
	{

		if (string.IsNullOrWhiteSpace(id)) return new();

		return await _userRepository.ViewUserByIdAsync(id);

	}

}
