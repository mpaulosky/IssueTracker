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
	private string _expectedHtml;
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
		_expectedHtml = @"
			<h1 class=""page-heading text-uppercase mb-4"">Issues</h1>
			<div class=""issue-container"">
				<button  class=""suggest-btn btn btn-outline-light btn-lg text-uppercase"">New Issue</button>
			</div>
			<div class=""row"">
				<div class=""issues-count col-md-4 text-light mt-2"">6 Issues</div>
				<div class=""col-md-4 col-xl-5 btn-group"">
					<button id=""sort-by-new"" class=""btn btn-order "" >New</button>
					<button id=""sort-by-popular"" class=""btn btn-order sort-selected"" >Popular</button>
				</div>
				<div class=""col-md-4 col-xl-3 search-box"">
					<input type=""text"" placeholder=""Search"" aria-label=""Search box"" class=""form-control rounded-control"" >
				</div>
				<div class=""col-12 d-block d-md-none"">
					<div class=""categories"" >
						<span class=""selected-category"">All</span>
					</div>
					<div class=""statuses"" >
						<span class=""selected-status"">All</span>
					</div>
				</div>
			</div>
			<div class=""row"">
				<div class=""col-md-8 col-xl-9"">
					<div style=""height: 0px; flex-shrink: 0;"" ></div>
        <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-design"">
          <div class=""issue-entry-category-text"" >Design</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 1</div>
          <div class=""issue-entry-text-description"">A new test issue 1</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">jim test</div>
            <div class=""issue-entry-text-category"">
              <button id=""archive""  class=""btn issue-entry-text-category btn-archive"">
                archive
              </button>
            </div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-watching"">
          <div class=""issue-entry-status-text"">Watching</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-documentation"">
          <div class=""issue-entry-category-text"" >Documentation</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 2</div>
          <div class=""issue-entry-text-description"">A new test issue 2</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">Tester</div>
            <div class=""issue-entry-text-category"">
              <button id=""archive""  class=""btn issue-entry-text-category btn-archive"">
                archive
              </button>
            </div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-answered"">
          <div class=""issue-entry-status-text"">Answered</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-implementation"">
          <div class=""issue-entry-category-text"" >Implementation</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 3</div>
          <div class=""issue-entry-text-description"">A new test issue 3</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">Tester</div>
            <div class=""issue-entry-text-category"">
              <button id=""archive""  class=""btn issue-entry-text-category btn-archive"">
                archive
              </button>
            </div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-inwork"">
          <div class=""issue-entry-status-text"">In Work</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-clarification"">
          <div class=""issue-entry-category-text"" >Clarification</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 3</div>
          <div class=""issue-entry-text-description"">A new test issue 3</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">jim test</div>
            <div class=""issue-entry-text-category"">
              <button id=""archive""  class=""btn issue-entry-text-category btn-archive"">
                archive
              </button>
            </div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-dismissed"">
          <div class=""issue-entry-status-text"">Dismissed</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-miscellaneous"">
          <div class=""issue-entry-category-text"" >Miscellaneous</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 3</div>
          <div class=""issue-entry-text-description"">A new test issue 3</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">Tester</div>
            <div class=""issue-entry-text-category"">
              <button id=""archive""  class=""btn issue-entry-text-category btn-archive"">
                archive
              </button>
            </div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-watching"">
          <div class=""issue-entry-status-text"">Watching</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-design"">
          <div class=""issue-entry-category-text"" >Design</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 6</div>
          <div class=""issue-entry-text-description"">A new test issue 6</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">Tester</div>
            <div class=""issue-entry-text-category"">
              <button id=""archive""  class=""btn issue-entry-text-category btn-archive"">
                archive
              </button>
            </div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-none"">
          <div class=""issue-entry-status-text""></div>
        </div>
      </div>
    </div>
			<div style=""height: 0px; flex-shrink: 0"" ></div>
			</div>
				<div diff:ignoreChildren diff:ignoreAttributes>
				</div>
			</div>";

		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		var cut = RenderComponent<Index>();

		// Assert
		cut.MarkupMatches(_expectedHtml);
	}

	[Fact]
	public void Index_With_DataNotAsAdmin_Should_DisplayIssuesWithOutArchiveButton_Test()
	{
		// Arrange
		_expectedHtml = @$"
			<h1 class=""page-heading text-uppercase mb-4"">Issues</h1>
			<div class=""issue-container"">
				<button  class=""suggest-btn btn btn-outline-light btn-lg text-uppercase"">New Issue</button>
			</div>
			<div class=""row"">
				<div class=""issues-count col-md-4 text-light mt-2"">6 Issues</div>
				<div class=""col-md-4 col-xl-5 btn-group"">
					<button id=""sort-by-new"" class=""btn btn-order "" >New</button>
					<button id=""sort-by-popular"" class=""btn btn-order sort-selected"" >Popular</button>
				</div>
				<div class=""col-md-4 col-xl-3 search-box"">
					<input type=""text"" placeholder=""Search"" aria-label=""Search box"" class=""form-control rounded-control"" >
				</div>
				<div class=""col-12 d-block d-md-none"">
					<div class=""categories"" >
						<span class=""selected-category"">All</span>
					</div>
					<div class=""statuses"" >
						<span class=""selected-status"">All</span>
					</div>
				</div>
			</div>
			<div class=""row"">
				<div class=""col-md-8 col-xl-9"">
					<div style=""height: 0px; flex-shrink: 0;"" ></div>
        <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-design"">
          <div class=""issue-entry-category-text"" >Design</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 1</div>
          <div class=""issue-entry-text-description"">A new test issue 1</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">jim test</div>
            <div class=""issue-entry-text-category""></div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-watching"">
          <div class=""issue-entry-status-text"">Watching</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-documentation"">
          <div class=""issue-entry-category-text"" >Documentation</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 2</div>
          <div class=""issue-entry-text-description"">A new test issue 2</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">Tester</div>
            <div class=""issue-entry-text-category""></div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-answered"">
          <div class=""issue-entry-status-text"">Answered</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-implementation"">
          <div class=""issue-entry-category-text"" >Implementation</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 3</div>
          <div class=""issue-entry-text-description"">A new test issue 3</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">Tester</div>
            <div class=""issue-entry-text-category""></div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-inwork"">
          <div class=""issue-entry-status-text"">In Work</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-clarification"">
          <div class=""issue-entry-category-text"" >Clarification</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 3</div>
          <div class=""issue-entry-text-description"">A new test issue 3</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">jim test</div>
            <div class=""issue-entry-text-category""></div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-dismissed"">
          <div class=""issue-entry-status-text"">Dismissed</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-miscellaneous"">
          <div class=""issue-entry-category-text"" >Miscellaneous</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 3</div>
          <div class=""issue-entry-text-description"">A new test issue 3</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">Tester</div>
            <div class=""issue-entry-text-category""></div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-watching"">
          <div class=""issue-entry-status-text"">Watching</div>
        </div>
      </div>
    </div>
    <div class=""issue-container"">
      <div class=""issue-entry"">
        <div class=""issue-entry-category issue-entry-category-design"">
          <div class=""issue-entry-category-text"" >Design</div>
        </div>
        <div class=""issue-entry-text"">
          <div class=""issue-entry-text-title"" >Test Issue 6</div>
          <div class=""issue-entry-text-description"">A new test issue 6</div>
          <div class=""issue-entry-bottom"">
            <div class=""issue-entry-text-category"" diff:ignoreAttributes>11.13.2022</div>
            <div class=""issue-entry-text-author"">Tester</div>
            <div class=""issue-entry-text-category""></div>
          </div>
        </div>
        <div class=""issue-entry-status issue-entry-status-none"">
          <div class=""issue-entry-status-text""></div>
        </div>
      </div>
    </div>
			<div style=""height: 0px; flex-shrink: 0"" ></div>
			</div>
				<div diff:ignoreChildren diff:ignoreAttributes>
				</div>
			</div>";

		SetUpTests(true, false, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		var cut = RenderComponent<Index>();

		// Assert
		cut.MarkupMatches(_expectedHtml);
	}

	[Fact]
	public void Index_With_ClickingOnIssue_Should_NavigateToDetailsPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/Details";

		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		var cut = RenderComponent<Index>();

		cut.FindAll("div.issue-entry-text-title")[0].Click();

		// Assert
		var navMan = Services.GetRequiredService<FakeNavigationManager>();
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
		var cut = RenderComponent<Index>();
		cut.FindAll("button")[0].Click();

		// Assert
		var navMan = Services.GetRequiredService<FakeNavigationManager>();
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
		var cut = RenderComponent<Index>();

		cut.FindAll("button")[0].Click();

		// Assert
		var navMan = Services.GetRequiredService<FakeNavigationManager>();
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
		var cut = RenderComponent<Index>();

		var buttons = cut.FindAll("#archive");
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
	public void Index_With_SelectingACategory_Should_FilterIssues_Test(int index, string expectedCategory)
	{
		// Arrange
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		var cut = RenderComponent<Index>();

		cut.FindAll("div.categories > div")[index].Click();

		// Assert
		_sessionStorageService.GetItemAsync<string>("_selectedCategory").Result.Should().Be(expectedCategory);
	}

	[Theory]
	[InlineData(0, "All")]
	[InlineData(1, "Answered")]
	[InlineData(2, "Watching")]
	[InlineData(3, "In Work")]
	[InlineData(4, "Dismissed")]
	public void Index_With_SelectingAStatus_Should_FilterTheIssues_Test(int index, string expectedStatus)
	{
		// Arrange
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		var cut = RenderComponent<Index>();

		cut.FindAll("div.statuses > div")[index].Click();

		// Assert
		_sessionStorageService.GetItemAsync<string>("_selectedStatus").Result.Should().Be(expectedStatus);
	}

	[Fact]
	public void Index_With_SelectingSortByNewest_Should_OrderIssuesNewestFirst_Test()
	{
		// Arrange
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		var cut = RenderComponent<Index>();

		cut.Find("#sort-by-new").Click();

		// Assert
		_sessionStorageService.GetItemAsync<bool>("_isSortedByNew").Result.Should().BeTrue();
	}

	[Fact]
	public void Index_With_SelectingSortByPopular_Should_OrderIssuesByPopularity_Test()
	{
		// Arrange
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		var cut = RenderComponent<Index>();

		cut.Find("#sort-by-popular").Click();

		// Assert
		_sessionStorageService.GetItemAsync<bool>("_isSortedByNew").Result.Should().BeFalse();
	}

	[Fact]
	public void Index_With_EnterSearchText_Should_FilterByText_Test()
	{
		// Arrange
		SetUpTests(true, true, false);

		_sessionStorageService = this.AddBlazoredSessionStorage();

		// Act
		var cut = RenderComponent<Index>();

		cut.Find("input").Input("test");

		// Assert
		_sessionStorageService.GetItemAsync<string>("_searchText").Result.Should().Be("test");
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
		if (isAuth == false)
		{
			this.AddTestAuthorization();
			return;
		}

		var authContext = this.AddTestAuthorization();
		authContext.SetAuthorized(_expectedUser.DisplayName);

		if (difUser == false)
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