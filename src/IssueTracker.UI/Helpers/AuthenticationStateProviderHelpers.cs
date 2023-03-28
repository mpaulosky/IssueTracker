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
		IUserService userService)
	{
		AuthenticationState authState = await provider.GetAuthenticationStateAsync();

		if (authState != null)
		{
			var objectId = authState.User.Claims
				.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;

			if (objectId != null)
			{
				return await userService.GetUserFromAuthentication(objectId);
			}
		}

		return new();
	}
}