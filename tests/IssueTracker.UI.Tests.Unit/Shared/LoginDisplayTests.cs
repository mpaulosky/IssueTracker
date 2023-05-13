using IssueTracker.UI.Shared;

namespace IssueTracker.UI.Tests.Unit.Shared;

[ExcludeFromCodeCoverage]
public class LoginDisplayTests
{

	[Fact]
	public void LoginDisplay_WithOut_Authorization_Should_DisplayLoginLink_Test()
	{

		// Arrange
		using var ctx = new TestContext();
		ctx.AddTestAuthorization();

		// Act
		IRenderedComponent<LoginDisplay> cut = ctx.RenderComponent<LoginDisplay>();

		// Assert
		cut.MarkupMatches("<a class='login-link' href='MicrosoftIdentity/Account/SignIn'>Login</a>");

	}

	[Fact]
	public void LoginDisplay_With_AuthenticationAndAuthorization_Should_DisplayProfileAndLogoutLinks_Test()
	{

		// Arrange
		using var ctx = new TestContext();
		TestAuthorizationContext authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized("TEST USER");

		// Act
		IRenderedComponent<LoginDisplay> cut = ctx.RenderComponent<LoginDisplay>();

		// Assert
		cut.MarkupMatches
		(
			"<a class='login-link' href='/Profile'>Profile</a><a class='login-link' href='MicrosoftIdentity/Account/SignOut'>Logout</a>"
		);

	}

	[Fact]
	public void LoginDisplay_With_AuthenticationAndAuthorizationAndPolicy_Should_DisplayAdminAndProfileAndLogoutLinks_Test()
	{

		// Arrange
		using var ctx = new TestContext();
		TestAuthorizationContext authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized("TEST USER");
		authContext.SetPolicies("Admin");

		// Act
		IRenderedComponent<LoginDisplay> cut = ctx.RenderComponent<LoginDisplay>();

		// Assert
		cut.MarkupMatches
		(
			@"
				<a class=""login-link"" href=""/Admin"">Admin</a><a class=""login-link"" href=""/Categories"">Categories</a><a class=""login-link"" href=""/Statuses"">Statuses</a><a class=""login-link"" href=""/Profile"">Profile</a><a class=""login-link"" href=""MicrosoftIdentity/Account/SignOut"">Logout</a>
			"
		);

	}

}
