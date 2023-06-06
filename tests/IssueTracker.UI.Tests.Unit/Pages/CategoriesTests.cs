namespace IssueTracker.UI.Pages;[ExcludeFromCodeCoverage]public class CategoriesTests : TestContext{	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;	private readonly IEnumerable<CategoryModel> _expectedCategories;	private readonly UserModel _expectedUser;	private readonly Mock<IMemoryCache> _memoryCacheMock;	private readonly Mock<ICacheEntry> _mockCacheEntry;	private readonly Mock<IUserRepository> _userRepositoryMock;	public CategoriesTests()	{		_categoryRepositoryMock = new Mock<ICategoryRepository>();		_userRepositoryMock = new Mock<IUserRepository>();		_memoryCacheMock = new Mock<IMemoryCache>();		_mockCacheEntry = new Mock<ICacheEntry>();		_expectedUser = FakeUser.GetNewUser(true);		_expectedCategories = FakeCategory.GetCategories(1);	}	[Fact]	public void Categories_CloseButton_Should_WhenClickedNavigateToIndexPage_Test()	{		// Arrange		const string expectedUri = "http://localhost/";		SetupMocks();		SetMemoryCache();		SetAuthenticationAndAuthorization(true, true);		RegisterServices();		// Act		var cut = RenderComponent<Categories>();		cut.Find("#close-page").Click();		// Assert		var navMan = Services.GetRequiredService<FakeNavigationManager>();		navMan.Uri.Should().NotBeNull();		navMan.Uri.Should().Be(expectedUri);	}	[Fact]	public void Categories_Should_DisplayMarkup_Test()	{		// Arrange		const string expectedHtml =			"""
			<h1 class="page-heading text-uppercase mb-4" >Categories</h1>
			<div class="row justify-content-center create-form" >
				<div class="form-layout col-xl-9 col-lg-11" >
					<div diff:ignore></div>
					<div class="" >
						<button type="button" class="rz-button rz-button-md rz-variant-filled rz-success rz-shade-default mt-2 mb-4" id:ignore >
							<span class="rz-button-box">
								<i class="rz-button-icon-left rzi">add_circle_outline</i>
								<span class="rz-button-text">Add New Category</span>
							</span>
						</button>
						<div class="rz-data-grid rz-has-paginator rz-datatable rz-datatable-scrollable " id:ignore >
							<div class="rz-data-grid-data">
								<table class="rz-grid-table rz-grid-table-fixed rz-grid-table-striped ">
									<colgroup>
										<col id:ignore style="width:120px">
										<col id:ignore style="width:200px">
										<col id:ignore style="width:156px">
									</colgroup>
									<thead>
										<tr>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column rz-text-align-left" scope="col" style="width:120px" >
												<div  tabindex="0" >
													<span class="rz-column-title" title="Category Name">
														<span class="rz-column-title-content">Category Name</span>
														<span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
													</span>
												</div>
											</th>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column rz-text-align-left" scope="col" style="width:200px" >
												<div  tabindex="0" >
													<span class="rz-column-title" title="Description">
														<span class="rz-column-title-content">Description</span>
														<span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
													</span>
												</div>
											</th>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-text-align-right" scope="col" style="width:156px;text-align:right;" >
												<div  tabindex="-1" >
													<span class="rz-column-title">
														<span class="rz-column-title-content"></span>
													</span>
												</div>
											</th>
										</tr>
									</thead>
									<tbody>
										<tr class="rz-data-row" diff:ignoreChildren >
											<td rowspan="1" colspan="1" style="width:120px"   >
												<span class="rz-cell-data" title="">
													Miscellaneous
												</span>
											</td>
											<td rowspan="1" colspan="1" style="width:200px"   >
												<span class="rz-cell-data" title="">
													Sit sunt porro.
												</span>
											</td>
											<td rowspan="1" colspan="1" style="width:156px;text-align:right;"   >
												<span class="rz-cell-data" title="">
													<button type="button" class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore  >
														<span class="rz-button-box">
															<i class="rz-button-icon-left rzi">edit</i>
														</span>
													</button>
													<button type="button" class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore  >
														<span class="rz-button-box">
															<i class="rz-button-icon-left rzi">delete</i>
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
			</div>
			""";		SetupMocks();		SetMemoryCache();		SetAuthenticationAndAuthorization(true, true);		RegisterServices();		// Act		var cut = RenderComponent<Categories>();		// Assert		cut.MarkupMatches(expectedHtml);	}	[Fact]	public void Categories_AddNewCategoryButton_Should_InsertNewInputRow_Test()	{		// Arrange		const string expectedHtml =			"""
			<h1 class="page-heading text-uppercase mb-4">Categories</h1>
			<div class="row justify-content-center create-form">
				<div class="form-layout col-xl-9 col-lg-11">
					<div class="close-button-section">
						<button id="close-page" class="btn btn-close"></button>
					</div>
					<div class="">
						<button disabled="" type="button" class="rz-button rz-button-md rz-variant-filled rz-success rz-shade-default rz-state-disabled mt-2 mb-4" id:ignore>
							<span class="rz-button-box">
								<i class="rz-button-icon-left rzi">add_circle_outline</i>
								<span class="rz-button-text">Add New Category</span>
							</span>
						</button>
						<div class="rz-data-grid rz-has-paginator rz-datatable  rz-datatable-scrollable " id:ignore>
							<div class="rz-data-grid-data">
								<table class="rz-grid-table rz-grid-table-fixed rz-grid-table-striped  ">
									<colgroup>
										<col id:ignore style="width:120px">
										<col id:ignore style="width:200px">
										<col id:ignore style="width:156px">
									</colgroup>
									<thead>
										<tr>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:120px">
												<div tabindex="0">
													<span class="rz-column-title" title="Category Name">
														<span class="rz-column-title-content">Category Name</span>
														<span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
													</span>
												</div>
											</th>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column rz-text-align-left" scope="col" style="width:200px">
												<div tabindex="0">
													<span class="rz-column-title" title="Description">
														<span class="rz-column-title-content">Description</span>
														<span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
													</span>
												</div>
											</th>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-text-align-right" scope="col" style="width:156px;text-align:right;">
												<div tabindex="-1">
													<span class="rz-column-title">
														<span class="rz-column-title-content"></span>
													</span>
												</div>
											</th>
										</tr>
									</thead>
									<tbody>
										<tr diff:ignore></tr>
										<tr diff:ignore></tr>
									</tbody>
								</table>
							</div>
						</div>
					</div>
				</div>
			</div>
			""";		SetupMocks();		SetMemoryCache();		SetAuthenticationAndAuthorization(true, true);		RegisterServices();		// Act		var cut = RenderComponent<Categories>();		cut.FindAll("button")[1].Click(); // Add New Category Button		// Assert		cut.MarkupMatches(expectedHtml);	}	[Fact]	public void Categories_AddNewCategoryButton_Should_CancelInsertOnCancel_Test()	{		// Arrange		const string expectedHtml =			"""
			<h1 class="page-heading text-uppercase mb-4" >Categories</h1>
			<div class="row justify-content-center create-form" >
				<div class="form-layout col-xl-9 col-lg-11" >
					<div class="close-button-section" >
						<button id="close-page" class="btn btn-close"  ></button>
					</div>
					<div class="" >
						<button type="button" class="rz-button rz-button-md rz-variant-filled rz-success rz-shade-default mt-2 mb-4" id:ignore >
							<span class="rz-button-box">
								<i class="rz-button-icon-left rzi">add_circle_outline</i>
								<span class="rz-button-text">Add New Category</span>
							</span>
						</button>
						<div class="rz-data-grid rz-has-paginator rz-datatable  rz-datatable-scrollable " id:ignore >
							<div class="rz-data-grid-data">
								<table class="rz-grid-table rz-grid-table-fixed rz-grid-table-striped ">
									<colgroup>
										<col id:ignore style="width:120px">
										<col id:ignore style="width:200px">
										<col id:ignore style="width:156px">
									</colgroup>
									<thead>
										<tr>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:120px" >
												<div  tabindex="0" >
													<span class="rz-column-title" title="Category Name">
														<span class="rz-column-title-content">Category Name</span>
														<span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
													</span>
												</div>
											</th>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:200px" >
												<div  tabindex="0" >
													<span class="rz-column-title" title="Description">
														<span class="rz-column-title-content">Description</span>
														<span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
													</span>
												</div>
											</th>
											<th rowspan="1" colspan="1" class="rz-unselectable-text     rz-text-align-right" scope="col" style="width:156px;text-align:right;" >
												<div  tabindex="-1" >
													<span class="rz-column-title">
														<span class="rz-column-title-content"></span>
													</span>
												</div>
											</th>
										</tr>
									</thead>
									<tbody>
										<tr class="rz-data-row  " diff:ignoreChildren >
											<td rowspan="1" colspan="1" style="width:120px"   >
												<span class="rz-cell-data" title="">
													Miscellaneous
												</span>
											</td>
											<td rowspan="1" colspan="1" style="width:200px"   >
												<span class="rz-cell-data" title="">
													Sit sunt porro.
												</span>
											</td>
											<td rowspan="1" colspan="1" style="width:156px;text-align:right;"   >
												<span class="rz-cell-data" title="">
													<button type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore  >
														<span class="rz-button-box">
															<i class="rz-button-icon-left rzi">edit</i>
														</span>
													</button>
													<button type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore  >
														<span class="rz-button-box">
															<i class="rz-button-icon-left rzi">delete</i>
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
			</div>
			""";		SetupMocks();		SetMemoryCache();		SetAuthenticationAndAuthorization(true, true);		RegisterServices();		// Act		var cut = RenderComponent<Categories>();		cut.FindAll("button")[1].Click(); // Add New Category Button		var buttons = cut.FindAll("button").ToList();		buttons[3].Click(); // Cancel Button		// Assert		cut.MarkupMatches(expectedHtml);	}	[Fact]	public void Categories_AddNewCategoryButton_Should_CreateNewCategory_Test()	{		// Arrange		SetupMocks();		SetMemoryCache();		SetAuthenticationAndAuthorization(true, true);		RegisterServices();		// Act		var cut = RenderComponent<Categories>();		cut.FindAll("button")[1].Click(); // Add New Category Button		var inputs = cut.FindAll("input");		inputs[0].Change("Test Name");		inputs[1].Change("Test Description");		var btns = cut.FindAll("button");		btns[2].Click(); // Click Submit Button		// Assert		_categoryRepositoryMock			.Verify(x =>				x.CreateAsync(It.IsAny<CategoryModel>()), Times.Once);	}	[Fact]	public void Categories_OnClickEditButton_Should_UpdateCategoryOnSubmit_Test()	{		// Arrange		SetupMocks();		SetMemoryCache();		SetAuthenticationAndAuthorization(true, true);		RegisterServices();		// Act		var cut = RenderComponent<Categories>();		cut.FindAll("button")[2].Click(); // Edit Button		var inputs = cut.FindAll("input");		inputs[0].Change("Test Name");		inputs[1].Change("Test Description");		var btns = cut.FindAll("button");		btns[2].Click(); // Click Submit Button		// Assert		_categoryRepositoryMock			.Verify(x =>				x.UpdateAsync(It.IsAny<string>(), It.IsAny<CategoryModel>()), Times.Once);	}	[Fact]	public void Categories_OnClickDeleteButton_Should_DeleteCategory_Test()	{		// Arrange		SetupMocks();		SetMemoryCache();		SetAuthenticationAndAuthorization(true, true);		RegisterServices();		// Act		var cut = RenderComponent<Categories>();		cut.FindAll("button")[3].Click(); // Delete Button		// Assert		_categoryRepositoryMock			.Verify(x =>				x.ArchiveAsync(It.IsAny<CategoryModel>()), Times.Once);	}	[Fact]	public void Categories_OnClickEditButton_Should_CancelEditOnCancelClick_Test()	{		// Arrange		const string expectedHtml =			"""
			<h1 class="page-heading text-uppercase mb-4" >Categories</h1>
			<div class="row justify-content-center create-form" >
				<div class="form-layout col-xl-9 col-lg-11" >
					<div class="close-button-section" >
						<button id="close-page" class="btn btn-close"  ></button>
					</div>
					<div class="" >
						<button type="button" class="rz-button rz-button-md rz-variant-filled rz-success rz-shade-default mt-2 mb-4" id:ignore >
							<span class="rz-button-box">
								<i class="rz-button-icon-left rzi">add_circle_outline</i>
								<span class="rz-button-text">Add New Category</span>
							</span>
						</button>
						<div class="rz-data-grid rz-has-paginator rz-datatable  rz-datatable-scrollable " id:ignore >
							<div class="rz-data-grid-data">
								<table class="rz-grid-table rz-grid-table-fixed rz-grid-table-striped ">
									<colgroup>
										<col id:ignore style="width:120px">
										<col id:ignore style="width:200px">
										<col id:ignore style="width:156px">
									</colgroup>
									<thead>
										<tr>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:120px" >
												<div  tabindex="0" >
													<span class="rz-column-title" title="Category Name">
														<span class="rz-column-title-content">Category Name</span>
														<span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
													</span>
												</div>
											</th>
											<th rowspan="1" colspan="1" class="rz-unselectable-text rz-sortable-column    rz-text-align-left" scope="col" style="width:200px" >
												<div  tabindex="0" >
													<span class="rz-column-title" title="Description">
														<span class="rz-column-title-content">Description</span>
														<span class="rz-sortable-column-icon rzi-grid-sort rzi-sort"></span>
													</span>
												</div>
											</th>
											<th rowspan="1" colspan="1" class="rz-unselectable-text     rz-text-align-right" scope="col" style="width:156px;text-align:right;" >
												<div  tabindex="-1" >
													<span class="rz-column-title">
														<span class="rz-column-title-content"></span>
													</span>
												</div>
											</th>
										</tr>
									</thead>
									<tbody>
										<tr class="rz-data-row  " diff:ignoreChildren >
											<td rowspan="1" colspan="1" style="width:120px"   >
												<span class="rz-cell-data" title="">
													Miscellaneous
												</span>
											</td>
											<td rowspan="1" colspan="1" style="width:200px"   >
												<span class="rz-cell-data" title="">
													Sit sunt porro.
												</span>
											</td>
											<td rowspan="1" colspan="1" style="width:156px;text-align:right;"   >
												<span class="rz-cell-data" title="">
													<button type="button"  class="rz-button rz-button-md rz-variant-flat rz-light rz-shade-default rz-button-icon-only" id:ignore  >
														<span class="rz-button-box">
															<i class="rz-button-icon-left rzi">edit</i>
														</span>
													</button>
													<button type="button"  class="rz-button rz-button-md rz-variant-flat rz-danger rz-shade-lighter rz-button-icon-only my-1 ms-1" id:ignore  >
														<span class="rz-button-box">
															<i class="rz-button-icon-left rzi">delete</i>
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
			</div>
			""";		SetupMocks();		SetMemoryCache();		SetAuthenticationAndAuthorization(true, true);		RegisterServices();		// Act		var cut = RenderComponent<Categories>();		var buttons = cut.FindAll("button").ToList();		buttons[2].Click(); // Edit Button		var buttons2 = cut.FindAll("button").ToList();		buttons2[3].Click(); // Click Cancel Button		// Assert		cut.MarkupMatches(expectedHtml);	}	private void SetupMocks()	{		_categoryRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_expectedCategories);		_userRepositoryMock.Setup(x => x.GetFromAuthenticationAsync(It.IsAny<string>())).ReturnsAsync(_expectedUser);	}	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)	{		var authContext = this.AddTestAuthorization();		if (isAuth)		{			authContext.SetAuthorized(_expectedUser.DisplayName);			authContext.SetClaims(				new Claim("objectidentifier", _expectedUser.Id)			);		}		if (isAdmin)		{			authContext.SetPolicies("Admin");		}	}	private void RegisterServices()	{		Services.AddSingleton<ICategoryService>(			new CategoryService(_categoryRepositoryMock.Object, _memoryCacheMock.Object));		Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));	}	private void SetMemoryCache()	{		_memoryCacheMock			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))			.Callback((object k) => _ = (string)k)			.Returns(_mockCacheEntry.Object);	}}
