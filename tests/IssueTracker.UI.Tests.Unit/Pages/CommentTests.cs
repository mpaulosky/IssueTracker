﻿using IssueTracker.UI.Pages;

namespace IssueTracker.UI.Tests.Unit.Pages;

[ExcludeFromCodeCoverage]
public class CommentTests : TestContext
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly IssueModel _expectedIssue;
	private readonly UserModel _expectedUser;

	public CommentTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
		_expectedUser = FakeUser.GetNewUser(true);
		_expectedIssue = FakeIssue.GetNewIssue(true);

	}

	[Fact]
	public void Comment_With_NullLoggedInUser_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		this.AddTestAuthorization();

		RegisterServices();

		// Act

		// Assert
		Assert.Throws<ArgumentNullException>(() => RenderComponent<Comment>()).Message.Should()
			.Be("Value cannot be null. (Parameter 'userObjectIdentifierId')");

	}

	[Fact]
	public void Comment_WithOut_IssueId_Should_ThrowArgumentNullExceptionOnInitialize_Test()
	{

		// Arrange
		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act

		// Assert
		Assert.Throws<ArgumentNullException>(() => RenderComponent<Comment>(parameter =>
		{

			parameter.Add(p => p.Id, null);

		}));

	}

	[Fact]
	public void Comment_CloseButton_Should_WhenClickedNavigateToIndexPage_Test()
	{

		// Arrange
		const string expectedUri = "http://localhost/";

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act
		IRenderedComponent<Comment> cut = RenderComponent<Comment>(parameter =>
		{

			parameter
				.Add(p => p.Id, _expectedIssue.Id);

		});
		cut.Find("#close-page").Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);

	}

	[Fact]
	public void Comment_With_ValidComment_Should_SaveTheComment_Test()
	{

		// Arrange
		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act
		IRenderedComponent<Comment> cut = RenderComponent<Comment>(parameter =>
		{

			parameter
				.Add(p => p.Id, _expectedIssue.Id);

		});

		cut.Find("#title").Change("Test Comment");
		cut.Find("#desc").Change("Test Description");
		cut.Find("#submit-comment").Click();

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<CommentModel>()), Times.Once);

	}

	private void SetupMocks()
	{

		_issueRepositoryMock.Setup(x => x.GetAsync(_expectedIssue.Id)).ReturnsAsync(_expectedIssue);
		_userRepositoryMock.Setup(x => x.GetFromAuthenticationAsync(It.IsAny<string>())).ReturnsAsync(_expectedUser);

	}

	private void SetAuthenticationAndAuthorization(bool isAdmin)
	{

		TestAuthorizationContext authContext = this.AddTestAuthorization();

		authContext.SetAuthorized(_expectedUser.DisplayName);

		authContext.SetClaims(new Claim("objectidentifier", _expectedUser.Id));

		if (isAdmin) authContext.SetPolicies("Admin");
	}

	private void RegisterServices()
	{

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));

		Services.AddSingleton<ICommentService>(new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object));

		Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));

	}

	private void SetMemoryCache()
	{

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

	}

}
