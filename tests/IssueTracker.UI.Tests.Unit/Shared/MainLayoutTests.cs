namespace IssueTracker.UI.Shared;

public class MainLayoutTests
{
	[Fact()]
	public void MainLayout_Should_DisplayMainLayout_Test()
	{
		// Arrange
		using var ctx = new TestContext();
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized("TEST USER");
		authContext.SetPolicies("Admin");

		// Act
		var cut = ctx.RenderComponent<MainLayout>();

		// Assert
		cut.MarkupMatches
		(
			@"
				<div class=""page"" >
				  <main class=""container-xxl"" >
				    <div class=""px-5 pt-3 nav-links"" >
				      <a class=""login-link"" href=""/Admin"">Admin</a>
				      <a class=""login-link"" href=""/Profile"">Profile</a>
				      <a class=""login-link"" href=""MicrosoftIdentity/Account/SignOut"">Logout</a>
				    </div>
				    <article class=""content pt-1"" ></article>
				  </main>
				</div>"
		);
	}
}