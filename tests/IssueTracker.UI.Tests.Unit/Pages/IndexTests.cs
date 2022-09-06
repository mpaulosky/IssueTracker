namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class IndexTests : TestContext
{
	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IStatusRepository> _statusRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;

	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	private UserModel _expectedUser;
	private List<IssueModel> _expectedIssues;
	private List<CategoryModel> _expectedCategories;
	private List<StatusModel> _expectedStatuses;

	public IndexTests()
	{
		_categoryRepositoryMock = new Mock<ICategoryRepository>();
		_statusRepositoryMock = new Mock<IStatusRepository>();
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}

	[Fact]
	public void Index_With_ValidData_Should_DisplayIssues_Test()
	{
		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssues = TestIssues.GetIssues().ToList();
		_expectedCategories = TestCategories.GetCategories().ToList();
		_expectedStatuses = TestStatuses.GetStatuses().ToList();

		SetupMocks();
		SetMemoryCache();

		// using var ctx = new TestContext();

		this.JSInterop.Setup<String>("localStorage.getItem", _ => true);

		SetAuthenticationAndAuthorization(this, true);
		RegisterServices(this);


		// Act
		var cut = RenderComponent<Index>();

		// Assert
		cut.MarkupMatches
			(
				@"
			<h1 class=""page-heading text-uppercase mb-4"">Issues</h1>
			<div class=""issue-container"">
				<button  class=""suggest-btn btn btn-outline-light btn-lg text-uppercase"">New Issue</button>
			</div>
			<div class=""row"">
				<div class=""issues-count col-md-4 text-light mt-2"">Issues</div>
				<div class=""col-md-4 col-xl-5 btn-group"">
					<button class=""btn btn-order sort-selected"" >New</button>
					<button class=""btn btn-order "" >Popular</button>
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
				<div class=""col-md-8 col-xl-9""></div>
				<div class=""col-md-4 col-xl-3 d-none d-md-block"">
					<div class=""categories"">
						<span class=""text-uppercase fw-bold"">Category</span>
						<div class=""selected-category"" >All</div>
						<div class="""" >Design</div>
						<div class="""" >Documentation</div>
						<div class="""" >Implementation</div>
						<div class="""" >Clarification</div>
						<div class="""" >Miscellaneous</div>
					</div>
					<div class=""statuses"">
						<span class=""text-uppercase fw-bold"">Status</span>
						<div class=""selected-status"" >All</div>
						<div class="""" >Answered</div>
						<div class="""" >Watching</div>
						<div class="""" >In Work</div>
						<div class="""" >Dismissed</div>
					</div>
				</div>
			</div>"
			);
	}

	private void SetupMocks()
	{
		_issueRepositoryMock
			.Setup(x => x.GetApprovedIssues())
			.ReturnsAsync(_expectedIssues);

		_userRepositoryMock
			.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>()))
			.ReturnsAsync(_expectedUser);

		_categoryRepositoryMock
			.Setup(x => x.GetCategories())
			.ReturnsAsync(_expectedCategories);

		_statusRepositoryMock
			.Setup(x => x.GetStatuses())
			.ReturnsAsync(_expectedStatuses);
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
		ctx.Services.AddSingleton<IStatusService>(new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<ICategoryService>(new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));
		ctx.Services.AddBlazoredLocalStorage();
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}