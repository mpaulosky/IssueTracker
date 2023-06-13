// Copyright (c) 2023. All rights reserved.
// File Name :     AuthenticationStateProviderHelpers.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI

namespace IssueTracker.UI.Helpers;

/// <summary>
///   AuthenticationStateProviderHelpers class
/// </summary>
public static class AuthenticationStateProviderHelpers
{
	/// <summary>
	///   Gets the user from authentication.
	/// </summary>
	/// <param name="provider">The AuthenticationState provider.</param>
	/// <param name="userData">The user service.</param>
	/// <returns>Task of Type UserModel</returns>
	public static async Task<UserModel> GetUserFromAuth(
		this AuthenticationStateProvider provider,
		IUserService userData)
	{
		AuthenticationState authState = await provider.GetAuthenticationStateAsync();

		string? objectId = authState.User.Claims
			.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;

		return await userData.GetUserFromAuthentication(objectId);
	}
}