// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     LoginDisplayTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

namespace IssueTracker.UI.Shared;

[ExcludeFromCodeCoverage]
public class LoginDisplayTests : TestContext
{
	private readonly UserModel _expectedUser;

	public LoginDisplayTests()
	{
		_expectedUser = FakeUser.GetNewUser(true);
	}

	[Fact]
	public void LoginDisplay_WithOut_Authorization_Should_DisplayLoginLink_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(false, false);

		// Act
		IRenderedComponent<LoginDisplay> cut = RenderComponent<LoginDisplay>();

		// Assert
		cut.MarkupMatches("<a class='login-link' href='MicrosoftIdentity/Account/SignIn'>Login</a>");
	}

	[Fact]
	public void LoginDisplay_With_AuthenticationAndAuthorization_Should_DisplayProfileAndLogoutLinks_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<LoginDisplay> cut = RenderComponent<LoginDisplay>();

		// Assert
		cut.MarkupMatches
		(
			"<a class='login-link' href='/Profile'>Profile</a><a class='login-link' href='MicrosoftIdentity/Account/SignOut'>Logout</a>"
		);
	}

	[Fact]
	public void
		LoginDisplay_With_AuthenticationAndAuthorizationAndPolicy_Should_DisplayAdminAndProfileAndLogoutLinks_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<LoginDisplay> cut = RenderComponent<LoginDisplay>();

		// Assert
		cut.MarkupMatches
		(
			@"
				<a class=""login-link"" href=""/Admin"">Admin</a><a class=""login-link"" href=""/Categories"">Categories</a><a class=""login-link"" href=""/Statuses"">Statuses</a><a class=""login-link"" href=""/Profile"">Profile</a><a class=""login-link"" href=""MicrosoftIdentity/Account/SignOut"">Logout</a>
			"
		);
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		TestAuthorizationContext authContext = this.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName);
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.Id)
			);
		}

		if (isAdmin)
		{
			authContext.SetPolicies("Admin");
		}
	}
}