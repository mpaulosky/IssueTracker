namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class CategoriesTest : TestContext
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;

	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	private IEnumerable<CategoryModel> _expectedCategories;
	private UserModel _expectedUser;

	public CategoriesTest()
	{

		_categoryRepositoryMock = new Mock<ICategoryRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

	}

	[Fact]
	public void Categories_Should_DisplayMarkup_Test()
	{
		// Arrange
		_expectedCategories = FakeCategory.GetCategories(1);
		_expectedUser = TestUsers.GetKnownUser();

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(isAdmin: true);

		RegisterServices();

		// Act
		var cut = RenderComponent<Categories>();

		// Assert
		cut.MarkupMatches(@$"
    <h1 class=""page-heading text-uppercase mb-4"" >Categories</h1>
		<div class=""row justify-content-center create-form"" >
			<div class=""form-layout col-xl-9 col-lg-11"" >
				<div class=""close-button-section"" >
					<button id=""new-category"" class=""btn category-entry-new-category""  >New Category</button>
					<button id=""close-page"" class=""btn btn-close""  ></button>
				</div>
				<div class="""" >
					<div style=""height: 0px;"" ></div>
					<div class=""category-entry"" >
						<div class=""category-entry-text-name"" diff:ignoreChildren >Implementation</div>
						<div class=""category-entry-text-description"" diff:ignoreChildren >Voluptas quis doloribus enim excepturi sunt consectetur maiores dolores excepturi.</div>
						<div >
							<button id=""edit-category"" class=""btn btn-primary""  >Edit</button>
						</div>
						<div >
							<button id=""delete-category"" class=""btn btn-primary""  >Delete</button>
						</div>
					</div>
					<div style=""height: 0px;"" ></div>
				</div>
			</div>
		</div>");

	}

	[Fact]
	public void Categories_With_NullLoggedInUser_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		this.AddTestAuthorization();

		RegisterServices();

		// Act

		// Assert
		Assert.Throws<ArgumentNullException>(() => RenderComponent<Categories>()).Message.Should()
			.Be("Value cannot be null. (Parameter 'userObjectIdentifierId')");

	}

	[Fact]
	public void Categories_CloseButton_Should_WhenClickedNavigateToIndexPage_Test()
	{

		// Arrange
		const string expectedUri = "http://localhost/";
		_expectedUser = TestUsers.GetKnownUser();
		_expectedCategories = FakeCategory.GetCategories(1);

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(isAdmin: true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Categories>();

		cut.Find("#close-page").Click();

		// Assert
		var navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);

	}

	private void SetupMocks()
	{

		_categoryRepositoryMock.Setup(x => x.GetCategories()).ReturnsAsync(_expectedCategories);

		_userRepositoryMock.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>())).ReturnsAsync(_expectedUser);

	}

	private void SetAuthenticationAndAuthorization(bool isAdmin)
	{

		var authContext = this.AddTestAuthorization();

		authContext.SetAuthorized(_expectedUser.DisplayName);

		authContext.SetClaims(
			new Claim("objectidentifier", _expectedUser.ObjectIdentifier)
		);

		if (isAdmin)
		{

			authContext.SetPolicies("Admin");

		}

	}

	private void RegisterServices()
	{

		Services.AddSingleton<ICategoryService>(new CategoryService(_categoryRepositoryMock.Object,
			_memoryCacheMock.Object));

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
