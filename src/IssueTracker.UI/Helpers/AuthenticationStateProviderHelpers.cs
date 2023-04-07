//-----------------------------------------------------------------------
// <copyright file="AuthenticationStateProviderHelpers.cs" company="mpaulosky">
//		Author: Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Helpers;

/// <summary>
///		AuthenticationStateProviderHelpers class
/// </summary>
public static class AuthenticationStateProviderHelpers
{
	/// <summary>
	///		Gets the user from authentication.
	/// </summary>
	/// <param name="provider">The AuthenticationState provider.</param>
	/// <param name="userService">The user service.</param>
	/// <returns>Task of Type UserModel</returns>
	public static async Task<UserModel> GetUserFromAuth(
		this AuthenticationStateProvider provider,
		IUserService userData)
	{

		var authState = await provider.GetAuthenticationStateAsync();
		string? objectId = authState.User.Claims
			.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
		return await userData.GetUserFromAuthentication(objectId);

	}

}
