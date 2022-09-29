namespace IssueTracker.UI.Shared;

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
		var cut = ctx.RenderComponent<LoginDisplay>();

		// Assert
		cut.MarkupMatches("<a class='login-link' href='MicrosoftIdentity/Account/SignIn'>Login</a>");
	}

	[Fact]
	public void LoginDisplay_With_AuthenticationAndAuthorization_Should_DisplayProfileAndLogoutLinks_Test()
	{
		// Arrange
		using var ctx = new TestContext();
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized("TEST USER");

		// Act
		var cut = ctx.RenderComponent<LoginDisplay>();

		// Assert
		cut.MarkupMatches
		(
			"<a class='login-link' href='/Profile'>Profile</a><a class='login-link' href='MicrosoftIdentity/Account/SignOut'>Logout</a>"
		);
	}

	[Fact]
	public void
		LoginDisplay_With_AuthenticationAndAuthorizationAndPolicy_Should_DisplayAminAndProfileAndLogoutLinks_Test()
	{
		// Arrange
		using var ctx = new TestContext();
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized("TEST USER");
		authContext.SetPolicies("Admin");

		// Act
		var cut = ctx.RenderComponent<LoginDisplay>();

		// Assert
		cut.MarkupMatches
		(
			@"
				<a class=""login-link"" href=""/Admin"">Admin</a><a class=""login-link"" href=""/Profile"">Profile</a><a class=""login-link"" href=""MicrosoftIdentity/Account/SignOut"">Logout</a>
			"
		);
	}
}