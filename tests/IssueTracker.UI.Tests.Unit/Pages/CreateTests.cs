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

	[Fact]
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

	[Fact()]
	public void Create_With_AuthorizedUser_Should_DisplayPage_Test()
	{
		// Arrange
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
		
		// Assert
		cut.MarkupMatches
			(
				@"<h1 class=""page-heading text-light text-uppercase mb-4"">Create An Issue</h1>
					<div class=""row justify-content-center create-form"">
					  <div class=""col-xl-8 col-lg-10 form-layout"">
					    <div class=""close-button-section"">
					      <button id=""close-page"" class=""btn btn-close"" ></button>
					    </div>
					    <form >
					      <div class=""input-section"">
					        <label class=""form-label fw-bold text-uppercase"" for=""issue-text"">Suggestion</label>
					        <div class=""input-description"">Focus on the topic or technology you want to learn about.</div>
					        <input id=""issue-text"" class=""form-control valid""  >
					      </div>
					      <div class=""input-section"">
					        <label class=""form-label fw-bold text-uppercase"" for=""category"">Category</label>
					        <div class=""input-description"">Choose one category.</div>
					        <div class=""col-lg-8"">
					          <div class=""radio-item-group"">
					            <input diff:ignore>
					            <label for=""5dc1039a1521eaa36835e541"">Design - An Issue with the design.</label>
					          </div>
					          <div class=""radio-item-group"">
					            <input diff:ignore>
					            <label for=""5dc1039a1521eaa36835e542"">Documentation - An Issue with the documentation.</label>
					          </div>
					          <div class=""radio-item-group"">
					            <input diff:ignore>
					            <label for=""5dc1039a1521eaa36835e543"">Implementation - An Issue with the implementation.</label>
					          </div>
					          <div class=""radio-item-group"">
					            <input diff:ignore>
					            <label for=""5dc1039a1521eaa36835e544"">Clarification - A quick Issue with a general question.</label>
					          </div>
					          <div class=""radio-item-group"">
					            <input diff:ignore>
					            <label for=""5dc1039a1521eaa36835e545"">Miscellaneous - Not sure where this fits.</label>
					          </div>
					        </div>
					      </div>
					      <div class=""input-section"">
					        <label class=""form-label fw-bold text-uppercase"" for=""description"">Description</label>
					        <div class=""input-description"">Briefly describe your suggestion.</div>
					        <textarea id=""description"" class=""form-control valid""  ></textarea>
					      </div>
					      <div class=""center-children"">
					        <button id=""submit"" class=""btn btn-main btn-lg text-uppercase"" type=""submit"">Creat Issue</button>
					      </div>
					    </form>
					  </div>
					</div>"
			);
	}

	[Fact()]
	public void Create_With_ValidInput_Should_SaveNewIssue_Test()
	{
		// Arrange
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

		cut.Find("#issue-text").Change("Test Issue");
		var inputs = cut.FindAll("input");
		inputs[1].Change("5dc1039a1521eaa36835e541");
		cut.Find("#description").Change("Test Description");
		cut.Find("#submit").Click();
		
		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.CreateIssue(It.IsAny<IssueModel>()), Times.Once);
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}