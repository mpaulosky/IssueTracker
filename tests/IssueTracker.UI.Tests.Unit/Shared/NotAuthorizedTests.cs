// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     NotAuthorizedTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

using AngleSharp.Dom;

namespace IssueTracker.UI.Shared;

[ExcludeFromCodeCoverage]
public class NotAuthorizedTests : TestContext
{
	[Fact]
	public void NotAuthorized_Should_DisplayMarkup_Test()
	{
		// Arrange

		// Act
		IRenderedComponent<NotAuthorized> cut = RenderComponent<NotAuthorized>();

		// Assert
		cut.MarkupMatches
		(
			@"<div class='row justify-content-center'>
				  <div class='col-xl-8 col-lg-10 form-layout'>
					  <div class='row'>
						  <div class='col-11'>
							  <div class='fw-bold mb-2 fs-5'>Authorization Required</div>
							  <p>
								  You are not authorized to access this section. You need to be logged in to submit new issues. You need to be an admin to manage the issues.
							  </p>
						  </div>
						  <div class='col-1 close-button-section'>
							  <button class='btn btn-close'></button>
						  </div>
					  </div>
				  </div>
			  </div>"
		);
	}

	[Fact]
	public void NotAuthorized_ClosePageButtonClick_Should_NavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();

		// Act
		IRenderedComponent<NotAuthorized> cut = RenderComponent<NotAuthorized>();

		IElement buttonElement = cut.Find("button");
		buttonElement.Click();

		// Assert
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}
}