// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     SolutionTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

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

	[Fact]
	public void Solution_WithOut_IssueId_Should_ThrowArgumentNullExceptionOnInitialize_Test()
	{
		// Arrange
		const string expectedParamName = "issueId";
		const string expectedMessage = "Value cannot be null.?*";

		SetAuthenticationAndAuthorization(false, true);

		// Act
		Func<IRenderedComponent<Solution>> cut = () => ComponentUnderTest(null);

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
			<h1 class="page-heading text-light text-uppercase mb-4" >Provide a Solution to an Issue</h1>
			<div class="issue-container" >
				<button id="create-comment"  class="suggest-btn btn btn-outline-light btn-lg text-uppercase" >
					Add Comment
				</button>
			</div>
			<div class="form-layout mb-3" >
				<div class="close-button-section" >
				 <button id="close-page" class="btn btn-close"  ></button>
				</div>
				<div class="issue-item-container" >
				 <div class="issue-entry-category issue-entry-category-clarification" >
				   <div class="issue-entry-category-text"  >Clarification</div>
				 </div>
				 <div class="issue-entry-text" >
					 <div diff:ignore></div>
					 <div diff:ignore></div>
				   <div class="issue-entry-bottom" >
					   <div diff:ignore></div>
					   <div diff:ignore></div>
				   </div>
				 </div>
				 <div class:ignore >
					 <div diff:ignore></div>
				 </div>
				</div>
				<form >
					<div class="input-section" >
						<label class="form-label fw-bold text-uppercase" for="title" >Title of the Solution</label>
						<div class="input-description" >Brief title of the Solution.</div>
						<textarea id="title" class="form-control valid"  ></textarea>
					</div>
					<div class="input-section" >
						<label class="form-label fw-bold text-uppercase" for="desc" >Give a solution for the issue.</label>
						<div class="input-description" >Give, in full your solution.</div>
							<textarea id="desc" class="form-control valid"  ></textarea>
						</div>
					<div diff:ignore></div>
				</form>
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

	[Fact]
	public void Solution_With_ValidComment_Should_SaveTheSolution_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<Solution> cut = ComponentUnderTest(_expectedIssue.Id);

		cut.Find("#title").Change("Test Solution");
		cut.Find("#desc").Change("Test Description");
		cut.Find("#submit-solution").Click();

		// Assert
		_solutionRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<SolutionModel>()), Times.Once);
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