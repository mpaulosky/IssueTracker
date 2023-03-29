namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class IndexTests : TestContext
{
	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IStatusRepository> _statusRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private List<CategoryModel> _expectedCategories;
	private List<IssueModel> _expectedIssues;
	private List<StatusModel> _expectedStatuses;

	private UserModel _expectedUser;
	private ISessionStorageService _sessionStorageService;

	public IndexTests()
	{
		_categoryRepositoryMock = new Mock<ICategoryRepository>();
		_statusRepositoryMock = new Mock<IStatusRepository>();
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
		_sessionStorageService = this.AddBlazoredSessionStorage();
	}

	[Theory]
	[InlineData("_selectedCategory", "All")]
	[InlineData("_selectedStatus", "All")]
	[InlineData("_searchText", "")]
	[InlineData("_isSortedByNew", "false")]
	public async Task Index_OnInitialize_Should_SaveSessionValues_Test(string key, string expectedValue)
	{
		// Arrange
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		RenderComponent<Index>();

		// Assert
		switch (key)
		{
			case "_isSortedByNew":
				var value = await _sessionStorageService.GetItemAsync<bool>(key);
				value.Should().Be((bool)Convert.ChangeType(expectedValue, typeof(bool)));
				break;
			default:
				var result = await _sessionStorageService.GetItemAsync<string>(key);
				result.Should().Be(expectedValue);
				break;
		}
	}

	[Fact]
	public void Index_With_DataAndAsAdmin_Should_DisplayIssuesWithArchiveButton_Test()
	{
		// Arrange
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();
		IRefreshableElementCollection<IElement> buttons = cut.FindAll("#archive");

		// Assert
		buttons.Count.Should().BeGreaterThan(0);
	}

	[Fact]
	public void Index_With_DataNotAsAdmin_Should_DisplayIssuesWithOutArchiveButton_Test()
	{
		// Arrange
		SetUpTests(true, false, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();
		IRefreshableElementCollection<IElement> buttons = cut.FindAll("#archive");

		// Assert
		buttons.Count.Should().Be(0);
	}

	[Fact]
	public void Index_With_ClickingOnIssue_Should_NavigateToDetailsPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/Details";

		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();

		cut.FindAll("div.issue-entry-text-title")[0].Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().StartWith(expectedUri);
	}

	[Fact]
	public void Index_With_ClickOfNewIssueButton_Should_NavigateToTheCreatePage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/Create";

		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();
		cut.FindAll("button")[0].Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact]
	public void Index_With_NotAuthenticatedAnClickCreateIssue_Should_NavigateToLoginPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/MicrosoftIdentity/Account/SignIn";

		SetUpTests(false, false, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();

		cut.FindAll("button")[0].Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact]
	public void Index_With_LoggedOnUserInfoIsDifferent_Should_UpdateUser_Test()
	{
		// Arrange
		SetUpTests(true, false, true);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		RenderComponent<Index>();

		// Assert
		_userRepositoryMock
			.Verify(x =>
				x.UpdateUser(It.IsAny<string>(), It.IsAny<UserModel>()), Times.Once);
	}

	[Fact]
	public void Index_With_ArchiveButtonClick_Should_UpdateIssueToArchived_Test()
	{
		// Arrange
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();

		IRefreshableElementCollection<IElement> buttons = cut.FindAll("#archive");
		buttons[0].Click();
		cut.Find("#confirm").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssue(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Theory]
	[InlineData(0, "All")]
	[InlineData(1, "Design")]
	[InlineData(2, "Documentation")]
	[InlineData(3, "Implementation")]
	[InlineData(4, "Clarification")]
	[InlineData(5, "Miscellaneous")]
	public async Task Index_With_SelectingACategory_Should_FilterIssues_Test(int index, string expected)
	{
		// Arrange
		const string sessionName = "_selectedCategory";
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();

		cut.FindAll("div.categories > div")[index].Click();

		// Assert
		var result = await _sessionStorageService.GetItemAsync<string>(sessionName);
		result.Should().Be(expected);
	}

	[Theory]
	[InlineData(0, "All")]
	[InlineData(1, "Answered")]
	[InlineData(2, "Watching")]
	[InlineData(3, "In Work")]
	[InlineData(4, "Dismissed")]
	public async Task Index_With_SelectingAStatus_Should_FilterTheIssues_TestAsync(int index, string expected)
	{
		// Arrange
		const string sessionName = "_selectedStatus";
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();

		cut.FindAll("div.statuses > div")[index].Click();

		// Assert
		var result = await _sessionStorageService.GetItemAsync<string>(sessionName);
		result.Should().Be(expected);
	}

	[Fact]
	public async Task Index_With_SelectingSortByNewest_Should_OrderIssuesNewestFirst_TestAsync()
	{
		// Arrange
		const string sessionName = "_isSortedByNew";
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();

		cut.Find("#sort-by-new").Click();

		// Assert
		var result = await _sessionStorageService.GetItemAsync<bool>(sessionName);
		result.Should().BeTrue();
	}

	[Fact]
	public async Task Index_With_SelectingSortByPopular_Should_OrderIssuesByPopularity_TestAsync()
	{
		// Arrange
		const string sessionName = "_isSortedByNew";
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();

		cut.Find("#sort-by-popular").Click();

		// Assert
		var result = await _sessionStorageService.GetItemAsync<bool>(sessionName);
		result.Should().BeFalse();
	}

	[Fact]
	public async Task Index_With_EnterSearchText_Should_FilterByText_TestAsync()
	{
		// Arrange
		const string sessionName = "_searchText";
		const string expected = "test";
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		IRenderedComponent<Index> cut = RenderComponent<Index>();

		cut.Find("input").Input("test");

		// Assert
		var result = await _sessionStorageService.GetItemAsync<string>(sessionName);
		result.Should().Be(expected);
	}

	[Fact]
	public void Index_With_NewUser_Should_SaveToDatabase_Test()
	{
		SetUpTests(true, false, false, true);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		RenderComponent<Index>();

		// Assert
		_userRepositoryMock
			.Verify(x =>
				x.CreateUser(It.IsAny<UserModel>()), Times.Once);
	}

	private void SetUpTests(bool isAuth, bool isAdmin, bool difUser, bool newUser = false)
	{
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssues = TestIssues.GetIssues().ToList();
		_expectedCategories = TestCategories.GetCategories().ToList();
		_expectedStatuses = TestStatuses.GetStatuses().ToList();

		SetupMocks();

		SetMemoryCache();

		SetAuthenticationAndAuthorization(isAuth, isAdmin, difUser, newUser);

		RegisterServices();
	}

	private void SetupMocks()
	{
		_issueRepositoryMock
			.Setup(x => x.GetApprovedIssues())
			.ReturnsAsync(_expectedIssues);

		_userRepositoryMock
			.Setup(x => x.GetUserFromAuthentication(_expectedUser.ObjectIdentifier))
			.ReturnsAsync(_expectedUser);

		_userRepositoryMock
			.Setup(x => x.GetUserFromAuthentication("5dc1039a1521eaa36835e547"))
			.ReturnsAsync(new UserModel());

		_categoryRepositoryMock
			.Setup(x => x.GetCategories())
			.ReturnsAsync(_expectedCategories);

		_statusRepositoryMock
			.Setup(x => x.GetStatuses())
			.ReturnsAsync(_expectedStatuses);
	}

	private void SetAuthenticationAndAuthorization(bool isAuth, bool isAdmin, bool difUser, bool newUser = false)
	{
		if (!isAuth)
		{
			this.AddTestAuthorization();
			return;
		}

		TestAuthorizationContext authContext = this.AddTestAuthorization();
		authContext.SetAuthorized(_expectedUser.DisplayName);

		if (!difUser)
		{
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.ObjectIdentifier),
				new Claim("name", _expectedUser.DisplayName),
				new Claim("givenname", _expectedUser.FirstName),
				new Claim("surname", _expectedUser.LastName),
				new Claim("email", _expectedUser.EmailAddress)
			);
		}
		else
		{
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.ObjectIdentifier),
				new Claim("name", "Bob Tester"),
				new Claim("givenname", "Bob"),
				new Claim("surname", "Tester"),
				new Claim("email", "bob.tester@tester.com")
			);
		}

		if (newUser)
		{
			authContext.SetClaims(
				new Claim("objectidentifier", "5dc1039a1521eaa36835e547"),
				new Claim("name", "Bob Tester"),
				new Claim("givenname", "Bob"),
				new Claim("surname", "Tester"),
				new Claim("email", "bob.tester@tester.com"));
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
		Services.AddSingleton<IStatusService>(new StatusService(_statusRepositoryMock.Object,
			_memoryCacheMock.Object));
		Services.AddSingleton<ICategoryService>(new CategoryService(_categoryRepositoryMock.Object,
			_memoryCacheMock.Object));
		Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));
		Services.AddBlazoredSessionStorage();
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}