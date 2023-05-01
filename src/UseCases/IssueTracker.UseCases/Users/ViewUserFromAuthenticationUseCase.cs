﻿//-----------------------------------------------------------------------
// <copyright File="ViewUserFromAuthenticationUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Users;

public class ViewUserFromAuthenticationUseCase : IViewUserFromAuthenticationUseCase
{

	private readonly IUserRepository _userRepository;

	public ViewUserFromAuthenticationUseCase(IUserRepository userRepository)
	{

		_userRepository = userRepository;

	}

	public async Task<UserModel?> ExecuteAsync(string userObjectIdentifierId)
	{

		Guard.Against.NullOrWhiteSpace(userObjectIdentifierId, nameof(userObjectIdentifierId));

		return await _userRepository.GetByAuthenticationIdAsync(userObjectIdentifierId);

	}

}
