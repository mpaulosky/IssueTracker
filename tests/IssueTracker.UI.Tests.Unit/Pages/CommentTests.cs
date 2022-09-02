namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class CommentTests
{
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private IssueModel _expectedIssue;
	private UserModel _expectedUser;

	public CommentTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}
	
	[Fact]
	public void Comment_With_NullLoggedInUser_Should_ThrowArgumentNullException_Test()
	{
		// Arrange
		using var ctx = new TestContext();

		ctx.AddTestAuthorization();

		RegisterServices(ctx);

		// Act

		// Assert
		Assert.Throws<ArgumentNullException>(() => ctx.RenderComponent<Comment>()).Message.Should().Be("Value cannot be null. (Parameter 'userId')");
		
	}

	[Fact]
	public void Comment_WithOut_IssueId_Should_ThrowArgumentNullExceptionOnInitialize_Test()
	{
		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetKnownIssue();

		SetupMocks();
		SetMemoryCache();

		using var ctx = new TestContext();

		SetAuthenticationAndAuthorization(ctx, false);
		RegisterServices(ctx);

		// Act

		// Assert
		Assert.Throws<ArgumentNullException>(() => ctx.RenderComponent<Comment>((parameter =>
		{
			parameter.Add(p => p.Id, null);
		})));
	}

	[Fact]
	public void Comment_CloseButton_Should_WhenClickedNavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetKnownIssue();

		SetupMocks();
		SetMemoryCache();

		using var ctx = new TestContext();

		SetAuthenticationAndAuthorization(ctx, false);
		RegisterServices(ctx);

		// Act
		var cut = ctx.RenderComponent<Comment>((parameter =>
		{
			parameter
				.Add(p => p.Id, _expectedIssue.Id);
		}));
		cut.Find("#close-page").Click();

		// Assert
		var navMan = ctx.Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact()]
	public void Comment_With_ValidComment_Should_SaveTheComment_Test()
	{
		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetKnownIssue();

		SetupMocks();
		SetMemoryCache();

		using var ctx = new TestContext();

		SetAuthenticationAndAuthorization(ctx, false);
		RegisterServices(ctx);

		// Act
		var cut = ctx.RenderComponent<Comment>((parameter =>
		{
			parameter
				.Add(p => p.Id, _expectedIssue.Id);
		}));

		cut.Find("#comment").Change("Test Comment");
		cut.Find("#submit-comment").Click();

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.CreateComment(It.IsAny<CommentModel>()), Times.Once);
	}

	private void SetupMocks()
	{
		_issueRepositoryMock.Setup(x => x.GetIssue(_expectedIssue.Id)).ReturnsAsync(_expectedIssue);

		_userRepositoryMock.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>())).ReturnsAsync(_expectedUser);
	}

	private void SetAuthenticationAndAuthorization(TestContext ctx, bool isAdmin)
	{
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized(_expectedUser.DisplayName);
		authContext.SetClaims(
			new Claim(type: "objectidentifier", _expectedUser.Id)
		);
		if (isAdmin)
		{
			authContext.SetPolicies("Admin");
		}
	}

	private void RegisterServices(TestContext ctx)
	{
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<ICommentService>(new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}