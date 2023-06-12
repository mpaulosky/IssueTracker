// Copyright (c) 2023. All rights reserved.
// File Name :     MainLayoutTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit

namespace IssueTracker.UI.Shared;

[ExcludeFromCodeCoverage]
public class MainLayoutTests : TestContext
{
	private readonly UserModel _expectedUser;

	public MainLayoutTests()
	{
		_expectedUser = FakeUser.GetNewUser(true);
	}

	[Fact]
	public void MainLayout_Should_DisplayMainLayout_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<MainLayout> cut = RenderComponent<MainLayout>();

		// Assert
		cut.MarkupMatches
		(
			@"			<div class=""page"">
				<main class=""container-xxl"">
					<div class=""px-5 pt-3 nav-links"">
						<a class=""login-link"" href=""/Admin"">Admin</a>
						<a class=""login-link"" href=""/Categories"">Categories</a>
						<a class=""login-link"" href=""/Statuses"">Statuses</a>
						<a class=""login-link"" href=""/Profile"">Profile</a>
						<a class=""login-link"" href=""MicrosoftIdentity/Account/SignOut"">Logout</a>
					</div>
					<article class=""content pt-1""></article>
				</main>
			</div>"
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