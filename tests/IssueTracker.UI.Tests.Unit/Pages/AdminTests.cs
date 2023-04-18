namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class AdminTests : TestContext
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
		const string expectedCount = "0";
		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();

		// Assert
		cut.FindAll("div")[1].TextContent.Should().StartWith(expectedCount);

		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-uppercase mb-4"">Pending Issues</h1>
				<div class=""row"">
				  <div class=""issue-count col-8 text-light mt-2"">0 Issues</div>
				  <div class=""col-4 close-button-section"">
				    <button id=""close-page"" class=""btn btn-close"" ></button>
				  </div>
				</div>"
		);

	}

	[Fact]
	public void Admin_With_UnApprovedIssues_Should_DisplayTheIssueAndIssueCountOfOne_Test()
	{

		// Arrange
		const string expectedCount = "3";
		SetupRepositoryMock();
		SetMemoryCache();
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4">Pending Issues</h1>
			<div class="row">
				<div class="issue-count col-8 text-light mt-2">3 Issues</div>
				<div diff:ignore></div>
			</div>
			<div class="row issue">
				<div diff:ignore></div>
				<div diff:ignore></div>
			</div>
			<div class="row issue">
				<div diff:ignore></div>
				<div diff:ignore></div>
			</div>
			<div class="row issue">
				<div diff:ignore></div>
				<div diff:ignore></div>
			</div>
			""";

		// Register services
		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();

		// Assert
		cut.FindAll("div")[1].TextContent.Should().StartWith(expectedCount);
		cut.MarkupMatches(expectedHtml);

	}

	[Fact]
	public void Admin_With_ApprovedButtonClick_Should_SetApprovedToTrue_Test()
	{

		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();
		cut.Find("#approve-issue").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssueAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact]
	public void Admin_With_EditTitleSpanClick_Should_ShowIssueTitleEditTextBox_Test()
	{

		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4">Pending Issues</h1>
			<div diff:ignore></div>
			<div class="row issue">
			  <div diff:ignore></div>
			  <div class="col-lg-10 col-md-9 col-sm-8">
			    <div>
			      <form class="approval-edit-form" >
			        <input id="title-text" class="form-control approval-edit-field valid" value="Test Issue 1"  >
			        <button id="submit-edit" class="btn" type="submit">
			          <span class="oi oi-check issue-edit-approve"></span>
			        </button>
			        <button id="reject-edit" type="button" class="btn" >
			          <span class="oi oi-x issue-edit-reject"></span>
			        </button>
			      </form>
			    </div>
			    <div>A new test issue 1<span id="edit-description" class="oi oi-pencil issue-edit-icon" ></span>
			    </div>
			    <div>
			      <div class="text-author" diff:ignore>
			      </div>
			    </div>
			    <div>
			      <div class="issue-entry-bottom">
			        <div class="text-category">
			          Category: Design</div>
			      </div>
			    </div>
			  </div>
			</div>
			<div diff:ignore></div>
			<div diff:ignore></div>
			""";

		SetupRepositoryMock();
		SetMemoryCache();

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();
		cut.Find("#edit-title").Click();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

	[Fact]
	public void Admin_With_EditDescriptionSpanClick_Should_ShowIssueDescriptionEditTextBox_Test()
	{

		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4">Pending Issues</h1>
			<div diff:ignore></div>
			<div class="row issue">
			<div diff:ignore></div>
				<div class="col-lg-10 col-md-9 col-sm-8">
					<div>Test Issue 1<span id="edit-title" class="oi oi-pencil issue-edit-icon" ></span>
					</div>
					<div>
						<form class="approval-edit-form" >
							<input id="description-text" class="form-control approval-edit-field valid" value="A new test issue 1"  >
							<button id="submit-description" class="btn" type="submit">
							  <span class="oi oi-check issue-edit-approve"></span>
							</button>
							<button id="reject-description" type="button" class="btn" >
							  <span class="oi oi-x issue-edit-reject"></span>
							</button>
						</form>
					</div>
					<div diff:ignore></div>
					<div diff:ignore></div>
				</div>
			</div>
			<div diff:ignore></div>
			<div diff:ignore></div>
			""";

		SetupRepositoryMock();
		SetMemoryCache();

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();
		cut.Find("#edit-description").Click();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

	[Fact]
	public void Admin_With_EditIssueTitleSubmit_Should_SaveChanges_Test()
	{

		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();
		cut.Find("#edit-title").Click();
		cut.Find("#title-text").Change("Text Change");
		cut.Find("#submit-edit").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssueAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact]
	public void Admin_With_EditIssueTitleReject_Should_RevertTheChanges_Test()
	{

		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4">Pending Issues</h1>
			<div diff:ignore></div>
			<div class="row issue">
				<div class="col-lg-2 col-md-3 col-sm-4">
					<button id="approve-issue" class="btn btn-approve" >Approve</button>
					<button id="reject-issue" class="btn btn-reject" >Reject</button>
				</div>
				<div class="col-lg-10 col-md-9 col-sm-8">
					<div>Test Issue 1<span id="edit-title" class="oi oi-pencil issue-edit-icon" ></span>
					</div>
					<div>A new test issue 1<span id="edit-description" class="oi oi-pencil issue-edit-icon" ></span>
					</div>
					<div diff:ignore></div>
					<div diff:ignore></div>
				</div>
			</div>
			<div diff:ignore></div>
			<div diff:ignore></div>
			""";

		SetupRepositoryMock();
		SetMemoryCache();

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();
		cut.Find("#edit-title").Click();
		cut.Find("#reject-edit").Click();

		// Assert 
		cut.MarkupMatches(expectedHtml);

	}

	[Fact]
	public void Admin_With_EditIssueDescriptionSubmit_Should_SaveChanges_Test()
	{

		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();
		cut.Find("#edit-description").Click();
		cut.Find("#description-text").Change("Description Changed");
		cut.Find("#submit-description").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssueAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact]
	public void Admin_With_EditIssueDescriptionReject_Should_RevertTheChanges_Test()
	{

		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-uppercase mb-4">Pending Issues</h1>
			<div diff:ignore></div>
			<div class="row issue">
				<div class="col-lg-2 col-md-3 col-sm-4">
					<button id="approve-issue" class="btn btn-approve" >Approve</button>
					<button id="reject-issue" class="btn btn-reject" >Reject</button>
				</div>
				<div class="col-lg-10 col-md-9 col-sm-8">
					<div>Test Issue 1<span id="edit-title" class="oi oi-pencil issue-edit-icon" ></span>
					</div>
					<div>A new test issue 1<span id="edit-description" class="oi oi-pencil issue-edit-icon" ></span>
					</div>
					<div diff:ignore></div>
					<div diff:ignore></div>
				</div>
			</div>
			<div diff:ignore></div>
			<div diff:ignore></div>
			""";

		SetupRepositoryMock();
		SetMemoryCache();

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();
		cut.Find("#edit-description").Click();
		cut.Find("#description-text").Change("Description Changed");
		cut.Find("#reject-description").Click();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

	[Fact]
	public void Admin_With_RejectButtonClick_Should_SetRejectToTrue_Test()
	{

		// Arrange
		SetupRepositoryMock();
		SetMemoryCache();

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();
		cut.Find("#reject-issue").Click();

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssueAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact]
	public void Admin_ClosePageButtonClick_Should_NavigateToIndexPage_Test()
	{

		// Arrange
		const string expectedUri = "http://localhost/";

		// Register services
		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));

		// Act
		IRenderedComponent<Admin> cut = RenderComponent<Admin>();
		cut.Find("#close-page").Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);

	}

	private void SetupRepositoryMock()
	{

		IEnumerable<IssueModel> expected = TestIssues.GetIssues().Where(c => !c.ApprovedForRelease);
		_issueRepositoryMock.Setup(x => x.GetIssuesWaitingForApprovalAsync()).ReturnsAsync(expected);

	}

	private void SetMemoryCache()
	{

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

	}

}
