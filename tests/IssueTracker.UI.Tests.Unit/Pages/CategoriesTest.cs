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
			<button id=""close-page"" class=""btn btn-close""  ></button>
			</div>
			<div class="""" >
			<button type=""button"" class=""rz-button rz-button-md rz-variant-filled rz-success rz-shade-default mt-2 mb-4"" id:ignore  >
			<span class=""rz-button-box"">
			<i class=""rz-button-icon-left rzi"">add_circle_outline</i>
			<span class=""rz-button-text"">Add New Category</span>
			</span>
			</button>
			<div class=""rz-data-grid rz-has-paginator rz-datatable  rz-datatable-scrollable "" id:ignore >
			<div class=""rz-data-grid-data"">
			<table class=""rz-grid-table rz-grid-table-fixed rz-grid-table-striped "">
			<colgroup>
			<col id:ignore style=""width:120px"">
			<col id:ignore style=""width:200px"">
			<col id:ignore style=""width:156px"">
			</colgroup>
			<thead>
			<tr>
			<th rowspan=""1"" colspan=""1"" class=""rz-unselectable-text rz-sortable-column    rz-text-align-left"" scope=""col"" style=""width:120px"" >
			<div  tabindex=""0"" >
			<span class=""rz-column-title"" title=""Category Name"">
			<span class=""rz-column-title-content"">Category Name</span>
			<span class=""rz-sortable-column-icon rzi-grid-sort rzi-sort""></span>
			</span>
			</div>
			</th>
			<th rowspan=""1"" colspan=""1"" class=""rz-unselectable-text rz-sortable-column    rz-text-align-left"" scope=""col"" style=""width:200px"" >
			<div  tabindex=""0"" >
			<span class=""rz-column-title"" title=""Description"">
			<span class=""rz-column-title-content"">Description</span>
			<span class=""rz-sortable-column-icon rzi-grid-sort rzi-sort""></span>
			</span>
			</div>
			</th>
			<th rowspan=""1"" colspan=""1"" class=""rz-unselectable-text     rz-text-align-right"" scope=""col"" style=""width:156px;text-align:right;"" >
			<div  tabindex=""-1"" >
			<span class=""rz-column-title"">
			<span class=""rz-column-title-content""></span>
			</span>
			</div>
			</th>
			</tr>
			</thead>
			<tbody>
			<tr class=""rz-data-row  "">
			<td rowspan=""1"" colspan=""1"" style=""width:120px"" diff:ignoreChildren  >
			<span class=""rz-cell-data"" title="""">
			Documentation
			</span>
			</td>
			<td rowspan=""1"" colspan=""1"" style=""width:200px"" diff:ignoreChildren  >
			<span class=""rz-cell-data"" title="""">
			Nam rem sunt magni commodi sunt soluta quia dolores.
			</span>
			</td>
			<td rowspan=""1"" colspan=""1"" style=""width:156px;text-align:right;"" diff:ignoreChildren  >
			<span class=""rz-cell-data"" title="""">
			<button type=""button""  class=""rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only"" id:ignore  >
			<span class=""rz-button-box"">
			<i class=""rz-button-icon-left rzi"">edit</i>
			</span>
			</button>
			<button type=""button""  class=""rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1"" id:ignore  >
			<span class=""rz-button-box"">
			<i class=""rz-button-icon-left rzi"">delete</i>
			</span>
			</button>
			</span>
			</td>
			</tr>
			</tbody>
			</table>
			</div>
			</div>
			</div>
			</div>
			</div>");

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
