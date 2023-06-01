namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class CreateTests : TestContext
{
	private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
	private readonly List<CategoryModel> _expectedCategories;
	private readonly List<StatusModel> _expectedStatuses;
	private readonly UserModel _expectedUser;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IStatusRepository> _statusRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;

	public CreateTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_categoryRepositoryMock = new Mock<ICategoryRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();
		_statusRepositoryMock = new Mock<IStatusRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
		_expectedUser = FakeUser.GetNewUser(true);
		_expectedCategories = FakeCategory.GetCategories().ToList();
		_expectedStatuses = FakeStatus.GetStatuses().ToList();
	}

	private IRenderedComponent<Create> ComponentUnderTest()
	{
		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		var component = RenderComponent<Create>();

		return component;
	}

	[Fact]
	public void Create_With_NullLoggedInUser_Should_ThrowArgumentNullException_Test()
	{
		// Arrange
		const string expectedParamName = "userObjectIdentifierId";
		const string expectedMessage = "Value cannot be null.?*";

		SetAuthenticationAndAuthorization(false, false);

		// Act
		var cut = ComponentUnderTest;

		// Assert
		cut.Should()
			.Throw<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact]
	public void Create_ClosePageClick_Should_NavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";


		SetAuthenticationAndAuthorization(false, true);

		// Act
		var cut = ComponentUnderTest();
		cut.Find("#close-page").Click();

		// Assert
		var navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact]
	public void Create_With_AuthorizedUser_Should_DisplayPage_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-light text-uppercase mb-4">Create An Issue</h1>
			<div class="row justify-content-center create-form">
				<div class="col-xl-8 col-lg-10 form-layout">
					<div class="close-button-section">
						<button id="close-page" class="btn btn-close" ></button>
					</div>
					<form >
						<div class="input-section">
							<label class="form-label fw-bold text-uppercase" for="issue-title">Issue Title</label>
							<div class="input-description">Focus on the topic or technology you want to learn about.</div>
							<input id="issue-title" class="form-control valid"  >
						</div>
						<div class="input-section">
							<label class="form-label fw-bold text-uppercase" for="description">Issue Description</label>
							<div class="input-description">Briefly describe your suggestion.</div>
							<textarea id="description" class="form-control valid"></textarea>
						</div>
						<div class="input-section">
							<label class="form-label fw-bold text-uppercase" for="category">Category</label>
							<div class="input-description">Choose one category.</div>
							<div class="col-lg-8">
								<div class="radio-item-group">
								  <input diff:ignore>
								  <label diff:ignore></label>
								</div>
								<div class="radio-item-group">
								  <input diff:ignore>
								  <label diff:ignore></label>
								</div>
								<div class="radio-item-group">
								  <input diff:ignore>
								  <label diff:ignore></label>
								</div>
								<div class="radio-item-group">
								  <input diff:ignore>
								  <label diff:ignore></label>
								</div>
								<div class="radio-item-group">
								  <input diff:ignore>
								  <label diff:ignore></label>
								</div>
							</div>
						</div>
						<div class="center-children">
							<button id="submit" class="btn btn-main btn-lg text-uppercase" type="submit">Create Issue</button>
						</div>
					</form>
				</div>
			</div>
			""";


		SetAuthenticationAndAuthorization(false, true);

		// Act
		var cut = ComponentUnderTest();

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Create_With_ValidInput_Should_SaveNewIssue_Test()
	{
		// Arrange
		var category = _expectedCategories.First();

		SetAuthenticationAndAuthorization(false, true);

		// Act
		var cut = ComponentUnderTest();

		cut.Find("#issue-title").Change("Test Issue");
		cut.Find("#description").Change("Test Description");
		var inputs = cut.FindAll("input");
		inputs[1].Change(category.Id);
		cut.Find("#submit").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<IssueModel>()), Times.Once);
	}

	private void SetupMocks()
	{
		_categoryRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_expectedCategories);

		_statusRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_expectedStatuses);

		_userRepositoryMock.Setup(x => x.GetFromAuthenticationAsync(It.IsAny<string>())).ReturnsAsync(_expectedUser);
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		var authContext = this.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName);
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.Id)
			);
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

		Services.AddSingleton<ICategoryService>(new CategoryService(_categoryRepositoryMock.Object,
			_memoryCacheMock.Object));

		Services.AddSingleton<IStatusService>(new StatusService(_statusRepositoryMock.Object, _memoryCacheMock.Object));

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
