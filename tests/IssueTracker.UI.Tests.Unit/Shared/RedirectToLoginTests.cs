namespace IssueTracker.UI.Shared;

[ExcludeFromCodeCoverage]
public class RedirectToLoginTests : TestContext
{
	private readonly UserModel _expectedUser = FakeUser.GetNewUser(true);

	[Fact]
	public void RedirectToLogin_NavigatesToSignIn()
	{
		// Arrange
		const string expectedUri = "http://localhost/MicrosoftIdentity/Account/SignIn?returnUrl=http://localhost/";
		SetAuthenticationAndAuthorization(false, false);

		// Act
		RenderComponent<RedirectToLogin>();
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();

		// Assert
		navMan!.Uri.Should().NotBeNull();
		navMan!.Uri.Should().Be(expectedUri);
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		TestAuthorizationContext authContext = this.AddTestAuthorization();

		switch (isAuth)
		{
			case true:
				authContext.SetAuthorized(_expectedUser.DisplayName);
				authContext.SetClaims(
					new Claim("objectidentifier", _expectedUser.Id)
				);
				break;
		}

		switch (isAdmin)
		{
			case true:
				authContext.SetPolicies("Admin");
				break;
		}
	}
}