namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class CategoriesTests : TestContext
{

	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;

	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	private IEnumerable<CategoryModel> _expectedCategories;
	private UserModel _expectedUser;

	public CategoriesTests()
	{

		_categoryRepositoryMock = new Mock<ICategoryRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

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
						<button type=""button"" class=""rz-button rz-button-md rz-variant-filled rz-success rz-shade-default mt-2 mb-4"" id:ignore >
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
										<tr class=""rz-data-row  "" diff:ignoreChildren >
											<td rowspan=""1"" colspan=""1"" style=""width:120px""   >
												<span class=""rz-cell-data"" title="""">
													Miscellaneous
												</span>
											</td>
											<td rowspan=""1"" colspan=""1"" style=""width:200px""   >
												<span class=""rz-cell-data"" title="""">
													Sit sunt porro.
												</span>
											</td>
											<td rowspan=""1"" colspan=""1"" style=""width:156px;text-align:right;""   >
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
			</div>"
		);

	}

	[Fact]
	public void Categories_AddNewCategoryButton_Should_InserNewInputRow_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedCategories = FakeCategory.GetCategories(1);

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(isAdmin: true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Categories>();

		cut.FindAll("button")[1].Click(); // Add New Category Button

		// Assert
		cut.MarkupMatches(@$"
			<h1 class=""page-heading text-uppercase mb-4"" >Categories</h1>
			<div class=""row justify-content-center create-form"" >
				<div class=""form-layout col-xl-9 col-lg-11"" >
					<div class=""close-button-section"" >
						<button id=""close-page"" class=""btn btn-close""  ></button>
					</div>
					<div class="""" >
						<button disabled="""" type=""button"" class=""rz-button rz-button-md rz-variant-filled rz-success rz-shade-default rz-state-disabled mt-2 mb-4"" id:ignore >
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
										<tr class=""rz-data-row rz-datatable-edit "">
											<td rowspan=""1"" colspan=""1"" style=""width:120px""   >
												<span class=""rz-cell-data"" title="""">
													<input name=""CategoryName"" style=""width: 100%; display: block;"" class=""rz-textbox valid"" tabindex=""0"" autocomplete=""on"" id:ignore >
												</span>
											</td>
											<td rowspan=""1"" colspan=""1"" style=""width:200px""   >
												<span class=""rz-cell-data"" title="""">
													<input name=""CategoryDescription"" style=""width: 100%; display: block;"" class=""rz-textbox valid"" tabindex=""0"" autocomplete=""on"" id:ignore >
												</span>
											</td>
											<td rowspan=""1"" colspan=""1"" style=""width:156px;text-align:right;""   >
												<span class=""rz-cell-data"" title="""">
													<button type=""button"" class=""rz-button rz-button-md rz-variant-flat rz-success rz-shade-default rz-button-icon-only"" id:ignore >
														<span class=""rz-button-box"">
															<i class=""rz-button-icon-left rzi"">check</i>
														</span>
													</button>
													<button type=""button"" class=""rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only my-1 ms-1"" id:ignore >
														<span class=""rz-button-box"">
															<i class=""rz-button-icon-left rzi"">close</i>
														</span>
													</button>
													<button type=""button"" class=""rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1"" id:ignore  >
														<span class=""rz-button-box"">
															<i class=""rz-button-icon-left rzi"">delete</i>
														</span>
													</button>
												</span>
											</td>
										</tr>
										<tr class=""rz-data-row  "" diff:ignoreChildren>
											<td rowspan=""1"" colspan=""1"" style=""width:120px""   >
												<span class=""rz-cell-data"" title="""">
													Documentation
												</span>
											</td>
											<td rowspan=""1"" colspan=""1"" style=""width:200px""   >
												<span class=""rz-cell-data"" title="""">
													Corporis est harum.
												</span>
											</td>
											<td rowspan=""1"" colspan=""1"" style=""width:156px;text-align:right;""   >
												<span class=""rz-cell-data"" title="""">
													<button type=""button""  class=""rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only"" id=""vrZzb-ScJE""  >
														<span class=""rz-button-box"">
															<i class=""rz-button-icon-left rzi"">edit</i>
														</span>
													</button>
													<button type=""button""  class=""rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1"" id=""6crFtguHAU""  >
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
			</div>"
		);

	}

	[Fact]
	public void Categories_AddNewCategoryButton_Should_CreateNewCategory_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedCategories = FakeCategory.GetCategories(1);

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(isAdmin: true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Categories>();

		cut.FindAll("button")[1].Click(); // Add New Category Button

		var inputs = cut.FindAll("input");
		inputs[0].Change("Test Name");
		inputs[1].Change("Test Description");

		var btns = cut.FindAll("button");
		btns[2].Click(); // Click Submit Button

		// Assert
		_categoryRepositoryMock
			.Verify(x =>
				x.CreateCategory(It.IsAny<CategoryModel>()), Times.Once);

	}

	[Fact]
	public void Categories_OnClickEditButton_Should_UpdateCategoryOnSubmit_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedCategories = FakeCategory.GetCategories(1);

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(isAdmin: true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Categories>();

		cut.FindAll("button")[2].Click(); // Edit Button


		var inputs = cut.FindAll("input");
		inputs[0].Change("Test Name");
		inputs[1].Change("Test Description");

		var btns = cut.FindAll("button");
		btns[2].Click(); // Click Submit Button

		// Assert
		_categoryRepositoryMock
			.Verify(x =>
				x.UpdateCategory(It.IsAny<string>(), It.IsAny<CategoryModel>()), Times.Once);

	}

	[Fact]
	public void Categories_OnClickDeleteButton_Should_DeletCategory_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedCategories = FakeCategory.GetCategories(1);

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(isAdmin: true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Categories>();

		cut.FindAll("button")[3].Click(); // Delete Button

		// Assert
		_categoryRepositoryMock
			.Verify(x =>
				x.DeleteCategory(It.IsAny<CategoryModel>()), Times.Once);

	}

	[Fact]
	public void Categories_OnClickEditButton_Should_CancelEditOnCancelClick_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedCategories = FakeCategory.GetCategories(1);

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(isAdmin: true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Categories>();

		cut.FindAll("button")[2].Click(); // Edit Button

		cut.FindAll("button")[2].Click(); // Click Cancel Button

		// Assert
		cut.MarkupMatches(@$"
			<h1 class=""page-heading text-uppercase mb-4"" >Categories</h1>
			<div class=""row justify-content-center create-form"" >
				<div class=""form-layout col-xl-9 col-lg-11"" >
					<div class=""close-button-section"" >
						<button id=""close-page"" class=""btn btn-close""  ></button>
					</div>
					<div class="""" >
						<button type=""button"" class=""rz-button rz-button-md rz-variant-filled rz-success rz-shade-default mt-2 mb-4"" id:ignore >
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
										<tr class=""rz-data-row  "" diff:ignoreChildren >
											<td rowspan=""1"" colspan=""1"" style=""width:120px""   >
												<span class=""rz-cell-data"" title="""">
													Miscellaneous
												</span>
											</td>
											<td rowspan=""1"" colspan=""1"" style=""width:200px""   >
												<span class=""rz-cell-data"" title="""">
													Sit sunt porro.
												</span>
											</td>
											<td rowspan=""1"" colspan=""1"" style=""width:156px;text-align:right;""   >
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
			</div>"
		);

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
