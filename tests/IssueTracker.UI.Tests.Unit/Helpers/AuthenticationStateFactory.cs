// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     AuthenticationStateFactory.cs
// Company :       mpaulosky
// Author :        teqsl
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

namespace IssueTracker.UI.Helpers;

[ExcludeFromCodeCoverage]
public static class AuthenticationStateFactory
{
	public static AuthenticationState Create(bool isAuthenticated, bool isAdmin, UserModel user)
	{
		ClaimsIdentity identity = new(
			new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.ObjectIdentifier), new Claim(ClaimTypes.Name, user.DisplayName),
				new Claim(ClaimTypes.GivenName, user.FirstName), new Claim(ClaimTypes.Surname, user.LastName),
				new Claim(ClaimTypes.Email, user.EmailAddress)
			}, "test");

		if (isAdmin)
		{
			identity.AddClaim(new Claim("jobTitle", "Admin"));
		}

		ClaimsPrincipal principal = new(identity);

		if (!isAuthenticated)
		{
			principal = new ClaimsPrincipal();
		}

		return new AuthenticationState(principal);
	}
}