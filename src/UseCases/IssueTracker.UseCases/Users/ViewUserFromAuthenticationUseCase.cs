//-----------------------------------------------------------------------
// <copyright File="ViewUserFromAuthenticationUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UseCases.PlugInRepositoryInterfaces;

namespace IssueTracker.UseCases.Users;

public class ViewUserFromAuthenticationUseCase : IViewUserFromAuthenticationUseCase
{

	private readonly IUserRepository _userRepository;

	public ViewUserFromAuthenticationUseCase(IUserRepository userRepository)
	{

		_userRepository = userRepository;

	}

	public async Task<UserModel> Execute(string userObjectIdentifierId)
	{

		if (string.IsNullOrWhiteSpace(userObjectIdentifierId)) return new();

		return await _userRepository.ViewUserByAuthenticationIdAsync(userObjectIdentifierId);

	}

}
