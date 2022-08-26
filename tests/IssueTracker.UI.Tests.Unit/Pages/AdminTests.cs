namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class AdminTests
{
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	public AdminTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}

	[Fact]
	public void Admin_With_No_Issues_Should_DisplayHeaderAndIssueCountOfZero_Test()
	{
		// Arrange
		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-uppercase mb-4"">Pending Issues</h1>
					<div class=""row"">
						<div class=""issue-count col-8 text-light mt-2"">0 Issues</div>
						<div class=""col-4 close-button-section"">
							<button class=""btn btn-close"" ></button>
						</div>
					</div>"
		);
	}

	[Fact]
	public void Admin_With_UnApprovedIssues_Should_DisplayTheIssueAndIssueCountOfOne_Test()
	{
		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-uppercase mb-4"">Pending Issues</h1>
				<div class=""row"">
					<div class=""issue-count col-8 text-light mt-2"">1 Issues</div>
					<div class=""col-4 close-button-section"">
					<button class=""btn btn-close"" ></button>
					</div>
					</div>
					<div class=""row issue"">
					<div class=""col-lg-2 col-md-3 col-sm-4"">
					<button class=""btn btn-approve"" >Approve</button>
					<button class=""btn btn-reject"" >Reject</button>
					</div>
					<div class=""col-lg-10 col-md-9 col-sm-8"">
					<div>Test Issue 2<span class=""oi oi-pencil issue-edit-icon"" ></span>
					</div>
					<div>A new test issue 2<span class=""oi oi-pencil issue-edit-icon"" ></span>
					</div>
					<div>
					<div class=""issue-entry-text-author"">
					Author: Tester</div>
					</div>
					<div>
					<span class=""text-muted"">Category: Unknown</span>
					</div>
					</div>
				</div>"
		);
	}

	[Fact]
	public void Admin_With_ApprovedButtonClick_Should_SetApprovedToTrue_Test()
	{
		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();
		var buttonElements = cut.FindAll("button");
		buttonElements[1].Click();

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-uppercase mb-4"">Pending Issues</h1>
			<div class=""row"">
			<div class=""issue-count col-8 text-light mt-2"">0 Issues</div>
			<div class=""col-4 close-button-section"">
			<button class=""btn btn-close""></button>
			</div>
			</div>"
		);

		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssue(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact]
	public void Admin_With_EditTitleSpanClick_Should_ShowIssueTitleEditTextBox_Test()
	{
		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();
		var spanElements = cut.FindAll("span");
		spanElements[0].Click();

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-uppercase mb-4"">Pending Issues</h1>
			<div class=""row"">
			<div class=""issue-count col-8 text-light mt-2"">1 Issues</div>
			<div class=""col-4 close-button-section"">
			<button class=""btn btn-close"" ></button>
			</div>
			</div>
			<div class=""row issue"">
			<div class=""col-lg-2 col-md-3 col-sm-4"">
			<button class=""btn btn-approve"" >Approve</button>
			<button class=""btn btn-reject"" >Reject</button>
			</div>
			<div class=""col-lg-10 col-md-9 col-sm-8"">
			<div>
			<form class=""approval-edit-form"" >
			<input class=""form-control approval-edit-field valid"" value=""Test Issue 2""  >
			<button class=""btn"" type=""submit"">
			<span class=""oi oi-check issue-edit-approve""></span>
			</button>
			<button type=""button"" class=""btn"" >
			<span class=""oi oi-x issue-edit-reject""></span>
			</button>
			</form>
			</div>
			<div>A new test issue 2<span class=""oi oi-pencil issue-edit-icon"" ></span>
			</div>
			<div>
			<div class=""issue-entry-text-author"">
			Author: Tester</div>
			</div>
			<div>
			<span class=""text-muted"">Category: Unknown</span>
			</div>
			</div>
			</div>"
		);
	}

	[Fact]
	public void Admin_With_EditDescriptionSpanClick_Should_ShowIssueDescriptionEditTextBox_Test()
	{
		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();
		var spanElements = cut.FindAll("span");
		spanElements[1].Click();

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-uppercase mb-4"">Pending Issues</h1>
			<div class=""row"">
			<div class=""issue-count col-8 text-light mt-2"">1 Issues</div>
			<div class=""col-4 close-button-section"">
			<button class=""btn btn-close"" ></button>
			</div>
			</div>
			<div class=""row issue"">
			<div class=""col-lg-2 col-md-3 col-sm-4"">
			<button class=""btn btn-approve"" >Approve</button>
			<button class=""btn btn-reject"" >Reject</button>
			</div>
			<div class=""col-lg-10 col-md-9 col-sm-8"">
			<div>Test Issue 2<span class=""oi oi-pencil issue-edit-icon"" ></span>
			</div>
			<div>
			<form class=""approval-edit-form"" >
			<input class=""form-control approval-edit-field valid"" value=""A new test issue 2""  >
			<button class=""btn"" type=""submit"">
			<span class=""oi oi-check issue-edit-approve""></span>
			</button>
			<button type=""button"" class=""btn"" >
			<span class=""oi oi-x issue-edit-reject""></span>
			</button>
			</form>
			</div>
			<div>
			<div class=""issue-entry-text-author"">
			Author: Tester</div>
			</div>
			<div>
			<span class=""text-muted"">Category: Unknown</span>
			</div>
			</div>
			</div>"
		);
	}

	[Fact]
	public void Admin_With_EditIssueTitleSubmit_Should_SaveChanges_Test()
	{
		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();
		var spanElements = cut.FindAll("span");
		spanElements[0].Click();
		var buttonElements = cut.FindAll("button");
		buttonElements[3].Click();

		// Assert
		//cut.MarkupMatches(@"");
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssue(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact]
	public void Admin_With_EditIssueTitleReject_Should_RevertTheChanges_Test()
	{
		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();
		var spanElements = cut.FindAll("span");
		spanElements[0].Click();
		var buttonElements = cut.FindAll("button");
		buttonElements[4].Click();

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-uppercase mb-4"">Pending Issues</h1>
					<div class=""row"">
					  <div class=""issue-count col-8 text-light mt-2"">1 Issues</div>
					  <div class=""col-4 close-button-section"">
					    <button class=""btn btn-close"" ></button>
					  </div>
					</div>
					<div class=""row issue"">
					  <div class=""col-lg-2 col-md-3 col-sm-4"">
					    <button class=""btn btn-approve"" >Approve</button>
					    <button class=""btn btn-reject"" >Reject</button>
					  </div>
					  <div class=""col-lg-10 col-md-9 col-sm-8"">
					    <div>Test Issue 2<span class=""oi oi-pencil issue-edit-icon"" ></span>
					    </div>
					    <div>A new test issue 2<span class=""oi oi-pencil issue-edit-icon"" ></span>
					    </div>
					    <div>
					      <div class=""issue-entry-text-author"">
					        Author: Tester</div>
					    </div>
					    <div>
					      <span class=""text-muted"">Category: Unknown</span>
					    </div>
					  </div>
					</div>"
		);
	}

	[Fact]
	public void Admin_With_EditIssueDescriptionSubmit_Should_SaveChanges_Test()
	{
		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();
		var spanElements = cut.FindAll("span");
		spanElements[1].Click();
		var buttonElements = cut.FindAll("button");
		buttonElements[3].Click();

		// Assert
		//cut.MarkupMatches(@"");
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssue(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact]
	public void Admin_With_EditIssueDescriptionReject_Should_RevertTheChanges_Test()
	{
		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();
		var spanElements = cut.FindAll("span");
		spanElements[1].Click();
		var buttonElements = cut.FindAll("button");
		buttonElements[4].Click();

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-uppercase mb-4"">Pending Issues</h1>
					<div class=""row"">
					  <div class=""issue-count col-8 text-light mt-2"">1 Issues</div>
					  <div class=""col-4 close-button-section"">
					    <button class=""btn btn-close"" ></button>
					  </div>
					</div>
					<div class=""row issue"">
					  <div class=""col-lg-2 col-md-3 col-sm-4"">
					    <button class=""btn btn-approve"" >Approve</button>
					    <button class=""btn btn-reject"" >Reject</button>
					  </div>
					  <div class=""col-lg-10 col-md-9 col-sm-8"">
					    <div>Test Issue 2<span class=""oi oi-pencil issue-edit-icon"" ></span>
					    </div>
					    <div>A new test issue 2<span class=""oi oi-pencil issue-edit-icon"" ></span>
					    </div>
					    <div>
					      <div class=""issue-entry-text-author"">
					        Author: Tester</div>
					    </div>
					    <div>
					      <span class=""text-muted"">Category: Unknown</span>
					    </div>
					  </div>
					</div>"
		);
	}

	[Fact]
	public void Admin_With_RejectButtonClick_Should_SetRejectToTrue_Test()
	{
		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();
		var buttonElements = cut.FindAll("button");
		buttonElements[2].Click();

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-uppercase mb-4"">Pending Issues</h1>
			<div class=""row"">
			<div class=""issue-count col-8 text-light mt-2"">0 Issues</div>
			<div class=""col-4 close-button-section"">
			<button class=""btn btn-close""></button>
			</div>
			</div>"
		);

		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssue(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact]
	public void Admin_ClosePageButtonClick_Should_NavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";

		using var ctx = new TestContext();

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));


		// Act
		var cut = ctx.RenderComponent<Admin>();
		var buttonElements = cut.FindAll("button");
		buttonElements[0].Click();

		// Assert
		var navMan = ctx.Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	private void SetupRepositoryMock()
	{
		var expected = TestIssues.GetIssues().Where(c => c.ApprovedForRelease == false);
		_issueRepositoryMock.Setup(x => x.GetIssuesWaitingForApproval()).ReturnsAsync(expected);
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}