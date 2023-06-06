namespace IssueTracker.UI.Components;

[ExcludeFromCodeCoverage]
public class IssueComponentTests : TestContext
{
	private readonly IssueModel _expectedIssue;
	private readonly UserModel _expectedUser;

	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IIssueService> _issueServiceMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	public IssueComponentTests()
	{
		_issueServiceMock = new Mock<IIssueService>();
		_issueRepositoryMock = new Mock<IIssueRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

		_expectedUser = FakeUser.GetNewUser(true);
		_expectedIssue = FakeIssue.GetNewIssue(true);
	}

	private IRenderedComponent<IssueComponent> ComponentUnderTest()
	{
		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		var component = RenderComponent<IssueComponent>(parameter =>
		{
			parameter.Add(p => p.Item, _expectedIssue);
			parameter.Add(p => p.LoggedInUser, _expectedUser);
		});

		return component;
	}

	[Fact(DisplayName = "IssueComponent not Admin")]
	public void IssueComponent_With_NotAdmin_Should_NotDisplaysArchiveButton_Test()
	{		// Arrange									const string expected ="""
			<div class="issue-item-container">
				<div class="issue-entry-category issue-entry-category-miscellaneous">
					<div class="issue-entry-category-text">Miscellaneous</div>
				</div>
				<div class="issue-entry-text">
					<div diff:ignore></div>
					<div diff:ignore></div>
					<div class="issue-entry-bottom">
						<div diff:ignore></div>
						<div diff:ignore></div>
					</div>
				</div>
				<div class="issue-entry-status issue-entry-status-inwork">
					<div class="issue-text-status">InWork</div>
				</div>
			</div>
			""";

		SetAuthenticationAndAuthorization(false, true);		// Act								var cut = ComponentUnderTest();		// Assert 										cut.MarkupMatches(expected);
	}

	[Fact(DisplayName = "IssueComponent is Admin")]
	public void IssueComponent_With_IsAdmin_Should_DisplaysArchiveButton_Test()
	{		// Arrange									const string expected ="""
			<div class="issue-item-container">
				<div class="issue-entry-category issue-entry-category-miscellaneous">
					<div diff:ignore></div>
				</div>
				<div class="issue-entry-text">
					<div diff:ignore></div>
					<div diff:ignore></div>
					<div class="issue-entry-bottom">
						<div diff:ignore></div>
						<div class="issue-text-archive">
							<button id="archive" class="btn issue-btn-archive">
								archive
							</button>
						</div>
						<div diff:ignore></div>
					</div>
				</div>
				<div class="issue-entry-status issue-entry-status-inwork">
					<div diff:ignore></div>
				</div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);		// Act								var cut = ComponentUnderTest();		// Assert 										cut.MarkupMatches(expected);
	}

	[Theory]
	[InlineData("Design", "issue-entry-category issue-entry-category-design")]
	[InlineData("Documentation", "issue-entry-category issue-entry-category-documentation")]
	[InlineData("Implementation", "issue-entry-category issue-entry-category-implementation")]
	[InlineData("Clarification", "issue-entry-category issue-entry-category-clarification")]
	[InlineData("Miscellaneous", "issue-entry-category issue-entry-category-miscellaneous")]
	[InlineData("", "issue-entry-category issue-entry-category-none")]
	public void IssueComponent_GetIssueCategoryCssClass_Should_Return_ValidCss_Test(string expectedCategory,
		string expectedCss)
	{		// Arrange									var model = new CategoryModel									{										Id = "test",										CategoryName = expectedCategory,										CategoryDescription = _expectedIssue.Category.CategoryDescription									};
		_expectedIssue.Category = new BasicCategoryModel(model);

		SetAuthenticationAndAuthorization(false, true);		// Act								var cut = ComponentUnderTest();
		var result = cut.Find("div.issue-entry-category");		// Assert 										result.ClassName.Should().Contain(expectedCss);

	}

	[Theory]
	[InlineData("Answered", "issue-entry-status issue-entry-status-answered")]
	[InlineData("InWork", "issue-entry-status issue-entry-status-inwork")]
	[InlineData("Watching", "issue-entry-status issue-entry-status-watching")]
	[InlineData("Dismissed", "issue-entry-status issue-entry-status-dismissed")]
	[InlineData("", "issue-entry-status issue-entry-status-none")]
	public void IssueComponent_GetIssueStatusCssClass_Should_Return_ValidCss_Test(string expectedStatus,
		string expectedCss)
	{		// Arrange									var model = new StatusModel									{										Id = "test",										StatusName = expectedStatus,										StatusDescription = _expectedIssue.IssueStatus.StatusDescription									};
		_expectedIssue.IssueStatus = new BasicStatusModel(model);

		SetAuthenticationAndAuthorization(false, true);		// Act								var cut = ComponentUnderTest();
		var result = cut.Find("div.issue-entry-status");		// Assert 										result.ClassName.Should().Contain(expectedCss);
	}

	[Fact]
	public void IssueComponent_OpenDetailsPage_Should_NavigateToDetailsPage_Test()
	{		// Arrange									var expectedUri = $"http://localhost/Details/{_expectedIssue.Id}";

		SetAuthenticationAndAuthorization(false, true);		// Act								var cut = ComponentUnderTest();

		cut.Find("div.issue-entry-category-text").Click();		// Assert										var navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact]
	public void IssueComponent_OpenDetailsPage2_Should_NavigateToDetailsPage_Test()
	{		// Arrange									var expectedUri = $"http://localhost/Details/{_expectedIssue.Id}";

		SetAuthenticationAndAuthorization(false, true);		// Act								var cut = ComponentUnderTest();

		cut.Find("div.issue-text-title").Click();		// Assert										var navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact]
	public void IssueComponent_ArchiveIssue_Should_ArchiveTheIssue_Test()
	{		// Arrange									SetAuthenticationAndAuthorization(true, true);		// Act								var cut = ComponentUnderTest();

		cut.Find("#archive").Click();
		cut.Find("#confirm").Click();		// Assert										_issueRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	private void SetupMocks()
	{
		_issueServiceMock.Setup(x => x
			.UpdateIssue(It.IsAny<IssueModel>())).Verifiable();
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		var authContext = this.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName);
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.ObjectIdentifier)
			);
		}

		if (isAdmin)
		{
			authContext.SetPolicies("Admin");
		}
	}


	private void RegisterServices()
	{
		Services.AddSingleton<IIssueService>(
			new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}
