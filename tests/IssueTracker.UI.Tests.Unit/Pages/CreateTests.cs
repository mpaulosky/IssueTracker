namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class CreateTests
{
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	public CreateTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_categoryRepositoryMock = new Mock<ICategoryRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}

	[Fact()]
	public void Create_ClosePageClick_Should_NavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";

		var expected = TestCategories.GetCategories;
		_categoryRepositoryMock.Setup(x => x.GetCategories()).ReturnsAsync(expected);

		var expectedUser = TestUsers.GetKnownUser();
		_userRepositoryMock.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>())).ReturnsAsync(expectedUser);

		SetMemoryCache();

		using var ctx = new TestContext();

		// Set Authentication and Authorization
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized(expectedUser.DisplayName);
		authContext.SetClaims(
			new Claim(type: "objectidentifier", expectedUser.Id)
		);

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<ICategoryService>(new CategoryService(_categoryRepositoryMock.Object,
			_memoryCacheMock.Object));
		ctx.Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));

		// Act
		var cut = ctx.RenderComponent<Create>();
		cut.Find("#close-page").Click();
		
		// Assert
		var navMan = ctx.Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}


	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}