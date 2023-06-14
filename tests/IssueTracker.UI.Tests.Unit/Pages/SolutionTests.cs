// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     SolutionTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

using IssueTracker.Services.Solution;
using IssueTracker.Services.Solution.Interface;

namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class SolutionTests : TestContext
{
	private readonly IssueModel _expectedIssue;
	private readonly UserModel _expectedUser;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;

	public SolutionTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_solutionRepositoryMock = new Mock<ISolutionRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

		_expectedUser = FakeUser.GetNewUser(true);
		_expectedIssue = FakeIssue.GetNewIssue(true);
	}

	private IRenderedComponent<Solution> ComponentUnderTest(string? issueId)
	{
		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		IRenderedComponent<Solution> component = RenderComponent<Solution>(parameter =>
		{
			parameter.Add(p => p.Id, issueId);
		});

		return component;
	}

	[Fact(DisplayName = "Solution Page Check Logged In User With Null User")]
	public void Solution_With_NullLoggedInUser_Should_ThrowAArgumentNullException_TestAsync()
	{
		// Arrange
		const string expectedParamName = "userObjectIdentifierId";
		const string expectedMessage = "Value cannot be null.?*";

		SetAuthenticationAndAuthorization(false, false);

		// Act
		Func<IRenderedComponent<Solution>> cut = () => ComponentUnderTest(_expectedIssue.Id);

		// Assert
		cut.Should()
			.Throw<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Solution With Valid User Should Display Markup")]
	public void Solution_With_ValidUser_Should_DisplayMarkup_TestAsync()
	{
		// Arrange
		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<Solution> cut = ComponentUnderTest(_expectedIssue.Id);

		// Assert
		cut.MarkupMatches
		(
			"""
				<h1 class="page-heading text-light text-uppercase mb-4">Solution to an Issue</h1>
			<div class="row justify-content-center create-form">
				<div class="col-xl-8 col-lg-10 form-layout">
					<div class="close-button-section">
						<button id="close-page" class="btn btn-close"></button>
					</div>
				</div>
			</div>
			"""
		);
	}

	[Fact(DisplayName = "Solution Close Button Should WhenClicked Navigate To Index Page")]
	public void Solution_CloseButton_Should_WhenClickedNavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<Solution> cut = ComponentUnderTest(_expectedIssue.Id);

		cut.Find("#close-page").Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	private void SetupMocks()
	{
		_issueRepositoryMock
			.Setup(x => x
				.GetAsync(_expectedIssue.Id))
			.ReturnsAsync(_expectedIssue);

		_userRepositoryMock
			.Setup(x => x
				.GetFromAuthenticationAsync(It.IsAny<string>()))
			.ReturnsAsync(_expectedUser);
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
		Services.AddSingleton<IIssueService>(
			new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));

		Services.AddSingleton<ISolutionService>(
			new SolutionService(_solutionRepositoryMock.Object, _memoryCacheMock.Object));

		Services.AddSingleton<IUserService>(
			new UserService(_userRepositoryMock.Object));
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}