// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     AdminTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

using AngleSharp.Dom;

namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class AdminTests : TestContext
{
	private readonly UserModel _expectedUser;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private IEnumerable<IssueModel> _expectedIssues;


	public AdminTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

		_expectedUser = FakeUser.GetNewUser(true);
		_expectedIssues = FakeIssue.GetIssues(1);
	}

	private IRenderedComponent<Admin> ComponentUnderTest()
	{
		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		IRenderedComponent<Admin> component = RenderComponent<Admin>();

		return component;
	}

	[Fact]
	public void Admin_With_No_Issues_Should_DisplayHeaderAndIssueCountOfZero_Test()
	{
		// Arrange
		const string expectedCount = "0";
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4">Pending Issues</h1>
			<div class="row">
				<div class="issue-count col-8 text-light mt-2">0 Issues</div>
				<div class="col-4 close-button-section">
					<button id = "close-page" class="btn btn-close"></button>
				</div>
			</div>
			""";

		_expectedIssues = new List<IssueModel>().AsEnumerable();

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();

		// Assert
		cut.FindAll("div")[1].TextContent.Should().StartWith(expectedCount);

		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Admin_With_UnApprovedIssues_Should_DisplayTheIssueAndIssueCountOfOne_Test()
	{
		// Arrange
		const string expectedCount = "1";
		const string expectedApprove = "Approve";
		const string expectedReject = "Reject";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();

		// Assert
		cut.FindAll("div")[1].TextContent.Should().StartWith(expectedCount);
		cut.Find("#approve-issue").TextContent.Should().Contain(expectedApprove);
		cut.Find("#reject-issue").TextContent.Should().Contain(expectedReject);
	}

	[Fact]
	public void Admin_With_ApprovedButtonClick_Should_SetApprovedToTrue_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();
		cut.Find("#approve-issue").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact]
	public void Admin_With_EditTitleSpanClick_Should_ShowIssueTitleEditTextBox_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();
		cut.Find("#edit-title").Click();

		// Assert
		cut.Find("#title-text").HasAttribute("value");
		cut.Find("#title-text").GetAttribute("value").Should().Be(_expectedIssues.First().Title);

		cut.Find("#submit-edit").HasAttribute("type");
		cut.Find("#submit-edit").GetAttribute("type").Should().Be("submit");
	}

	[Fact]
	public void Admin_With_EditDescriptionSpanClick_Should_ShowIssueDescriptionEditTextBox_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();
		cut.Find("#edit-description").Click();

		// Assert
		IElement expectedButton = cut.Find("#submit-description");
		expectedButton.HasAttribute("type");
		expectedButton.GetAttribute("type").Should().Be("submit");
	}

	[Fact]
	public void Admin_With_EditIssueTitleSubmit_Should_SaveChanges_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();

		cut.Find("#edit-title").Click();
		cut.Find("#title-text").Change("Text Change");
		cut.Find("#submit-edit").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact]
	public void Admin_With_EditIssueTitleReject_Should_RevertTheChanges_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4">Pending Issues</h1>
			<div class="row">
				<div class="issue-count col-8 text-light mt-2">1 Issues</div>
				<div class="col-4 close-button-section">
					<button id="close-page" class="btn btn-close"></button>
				</div>
			</div>
			<div class="row issue">
				<div class="col-lg-2 col-md-3 col-sm-4">
					<button id="approve-issue" class="btn btn-approve">Approve</button>
					<button id="reject-issue" class="btn btn-reject">Reject</button>
				</div>
				<div class="col-lg-10 col-md-9 col-sm-8">
					<div diff:ignore></div>
					<div diff:ignore></div>
					<div diff:ignore></div>
					<div diff:ignore></div>
				</div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();

		cut.Find("#edit-title").Click();
		cut.Find("#reject-edit").Click();

		// Assert 
		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Admin_With_EditIssueDescriptionSubmit_Should_SaveChanges_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();

		cut.Find("#edit-description").Click();
		cut.Find("#description-text").Change("Description Changed");
		cut.Find("#submit-description").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact]
	public void Admin_With_EditIssueDescriptionReject_Should_RevertTheChanges_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4">Pending Issues</h1>
			<div class="row">
				<div class="issue-count col-8 text-light mt-2">1 Issues</div>
				<div class="col-4 close-button-section">
					<button id="close-page" class="btn btn-close"></button>
				</div>
			</div>
			<div class="row issue">
				<div class="col-lg-2 col-md-3 col-sm-4">
					<button id="approve-issue" class="btn btn-approve">Approve</button>
					<button id="reject-issue" class="btn btn-reject">Reject</button>
				</div>
				<div class="col-lg-10 col-md-9 col-sm-8">
					<div diff:ignore></div>
					<div diff:ignore></div>
					<div diff:ignore></div>
					<div diff:ignore></div>
				</div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();

		cut.Find("#edit-description").Click();
		cut.Find("#description-text").Change("Description Changed");
		cut.Find("#reject-description").Click();

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Admin_With_RejectButtonClick_Should_SetRejectToTrue_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();

		cut.Find("#reject-issue").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact]
	public void Admin_ClosePageButtonClick_Should_NavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Admin> cut = ComponentUnderTest();
		cut.Find("#close-page").Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	private void SetupMocks()
	{
		foreach (IssueModel? issue in _expectedIssues)
		{
			issue.ApprovedForRelease = false;
			issue.Rejected = false;
			issue.Archived = false;
		}

		_issueRepositoryMock.Setup(x => x
				.GetWaitingForApprovalAsync())
			.ReturnsAsync(_expectedIssues);
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

	private void RegisterServices()
	{
		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}

}