namespace IssueTracker.UI.Tests.Unit.Shared;

[ExcludeFromCodeCoverage]
public class NotAuthorizedTests
{
	[Fact()]
	public void NotAuthorized_Should_DisplayMarkup_Test()
	{
		// Arrange
		using var ctx = new TestContext();

		// Act
		var cut = ctx.RenderComponent<NotAuthorized>();

		// Assert
		cut.MarkupMatches
		(
			@"<div class='row justify-content-center'>
					<div class='col-xl-8 col-lg-10 form-layout'>
					<div class='row'>
					<div class='col-11'>
					<div class='fw-bold mb-2 fs-5'>Authorization Required</div>
					<p>
					You are not authorized to access this section. You need to be logged in 					to submit new issues. You need to be an admin to manage the issues.
					</p>
					</div>
					<div class='col-1 close-button-section'>
					<button class='btn btn-close' ></button>
					</div>
					</div>
					</div>
					</div>"
		);
	}
}