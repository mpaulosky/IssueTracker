namespace IssueTracker.UI.Helpers;

public static class AuthenticationStateProviderHelpers
{
	public static async Task<UserModel> GetUserFromAuth(
		this AuthenticationStateProvider provider,
		IUserService userService)
	{
		var authState = await provider.GetAuthenticationStateAsync();

		string objectId = authState.User.Claims
			.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;

		return await userService.GetUserFromAuthentication(objectId);
	}
}