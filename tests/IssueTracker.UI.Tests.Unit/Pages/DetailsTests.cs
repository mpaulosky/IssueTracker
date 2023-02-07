namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class DetailsTests : TestContext
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IStatusRepository> _statusRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private List<CommentModel> _expectedComments;
	private IssueModel _expectedIssue;
	private List<StatusModel> _expectedStatuses;
	private UserModel _expectedUser;

	public DetailsTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

	}

	[Fact]
	public void Details_With_NullLoggedInUser_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		this.AddTestAuthorization();

		RegisterServices();

		// Act

		// Assert
		Assert.Throws<ArgumentNullException>(() => RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, null);
		})).Message.Should().Be("Value cannot be null. (Parameter 'userObjectIdentifierId')");

	}

	[Fact]
	public void Details_WithOut_IssueId_Should_ThrowArgumentNullExceptionOnInitialization_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetKnownIssue();

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act

		// Assert
		Assert.Throws<ArgumentNullException>(() => RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, null);
		}));

	}

	[Fact]
	public void Details_ClosePageClick_Should_NavigateToIndexPage_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		const string expectedUri = "http://localhost/";
		_expectedIssue = TestIssues.GetKnownIssue();
		_expectedComments = TestComments.GetComments().ToList();

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.Find("#close-page").Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);

	}

	[Fact]
	public void Details_With_NonAdminUser_Should_ShowDetailsNotSetStatus_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetKnownIssue();
		_expectedComments = TestComments.GetComments().ToList();
		const string _expectedHtml =
			"""
			<h1 class="page-heading text-light text-uppercase mb-4">Issue Details</h1>
			<div diff:ignore></div>
			<div class="form-layout mb-3">
				<div diff:ignore></div>
				<div class="issue-container">
					<div class="issue-entry">
						<div class="issue-entry-category issue-entry-category-miscellaneous">
							<div class="issue-entry-category-text" >Miscellaneous</div>
						</div>
						<div class="issue-entry-text">
							<div class="issue-entry-text-title" >Test Issue 1</div>
							<div class="issue-entry-text-description">A new test issue 1</div>
							<div class="issue-entry-bottom">
								<div class="issue-entry-text-category" diff:ignoreChildren>11.12.2022</div>
								<div class="issue-entry-text-author">Tester</div>
								<div class="issue-entry-text-category"></div>
							</div>
						</div>
						<div class="issue-entry-status issue-entry-status-watching">
							<div class="issue-entry-status-text">Watching</div>
						</div>
					</div>
				</div>
				<div diff:ignore>
				</div>
			</div>
			""";

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		// Assert
		cut.MarkupMatches(_expectedHtml);

	}

	[Fact]
	public void Details_With_AdminUser_Should_BeAbleToSetStatus_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetKnownIssue();
		_expectedComments = TestComments.GetComments().ToList();
		const string _expectedHtml =
		"""
		<h1 class="page-heading text-light text-uppercase mb-4">Issue Details</h1>
		<div diff:ignore></div>
		<div class="form-layout mb-3">
		  <div diff:ignore></div>
		  <div diff:ignore></div>
		  <div class="issue-container">
		    <div class="form-layout status-entry">
		      <div class="fw-bold mb-2">
		        Set Status
		      </div>
		      <div class="admin-set-statuses">
		        <button id="answered"  class="btn issue-entry-text-category btn-archive btn-status-answered">
		          answered
		        </button>
		        <button id="inwork"  class="btn issue-entry-text-category btn-archive btn-status-inwork">
		          in work
		        </button>
		        <button id="watching"  class="btn issue-entry-text-category btn-archive btn-status-watching">
		          watching
		        </button>
		        <button id="dismissed"  class="btn issue-entry-text-category btn-archive btn-status-dismissed">
		          dismissed
		        </button>
		      </div>
		    </div>
		  </div>
		  <div diff:ignore>
		  </div>
		</div>
		""";

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		// Assert
		cut.MarkupMatches(_expectedHtml);

	}

	[Fact]
	public void Details_With_AddCommentClick_Should_NavigateToCommentPage_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetKnownIssue();
		_expectedComments = TestComments.GetComments().ToList();

		SetupMocks();
		SetMemoryCache();

		var expectedUri = $"http://localhost/Comment/{_expectedIssue.Id}";

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.Find("#create-comment").Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);

	}

	[Theory(DisplayName = "Validate Status Styles")]
	[InlineData(0, "issue-entry-status-watching")]
	[InlineData(1, "issue-entry-status-answered")]
	[InlineData(2, "issue-entry-status-inwork")]
	[InlineData(3, "issue-entry-status-dismissed")]
	[InlineData(5, "issue-entry-status-none")]
	public void Details_With_ValidIssue_Should_ShowStatusStyle_Test(int index, string expected)
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetIssues().ToList()[index];
		_expectedComments = TestComments.GetComments().ToList();

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		IRefreshableElementCollection<IElement> results = cut.FindAll("div");
		var items = results.Select(x => x.ClassName).Where(z => z != null && z.Contains(expected)).ToList();

		// Assert
		items.Count.Should().Be(1);

	}

	[Fact]
	public void Details_When_CommentVotedOnNonAuthor_Should_SaveUpdatedComment_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetIssues().ToList()[0];
		_expectedComments = TestComments.GetComments().ToList();

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.FindAll("#vote")[0].Click();

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.UpVoteComment(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

	}

	[Fact]
	public void Details_WhenCommentHasVoteByUser_Should_RemoveVote_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetIssues().ToList()[0];
		_expectedComments = TestComments.GetComments().ToList();

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.FindAll("#vote")[2].Click();

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.UpVoteComment(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

	}

	[Fact]
	public void Details_With_ChangingAnsweredStatusWithoutUrl_Should_Fail_Test()
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetIssues().ToList()[5];
		_expectedComments = TestComments.GetComments().ToList();
		const string _expectedHtml =
			""""
			<h1 class="page-heading text-light text-uppercase mb-4">Issue Details</h1>
			<div diff:ignore></div>
			<div class="form-layout mb-3">
			  <div diff:ignore></div>
			  <div class="issue-container">
			    <div class="issue-entry">
			      <div diff:ignore></div>
			      <div diff:ignore></div>
			      <div class="issue-entry-status issue-entry-status-none">
			        <div class="issue-entry-status-text"></div>
			      </div>
			    </div>
			  </div>
			  <div class="issue-container">
			    <div class="form-layout status-entry">
			      <div class="fw-bold mb-2">
			        Set Status
			      </div>
			      <div>
			        <input id="input-answer" class="form-control rounded-control" type="text" placeholder="Url" aria-label="Content Url" value="" >
			      </div>
			      <div class="issue-entry-bottom">
			        <button id="confirm-answered-status" class="btn btn-archive-confirm" >
			          confirm
			        </button>
			        <button id="cancel-answered-status" class="btn btn-archive-reject" >
			          cancel
			        </button>
			      </div>
			    </div>
			  </div>
			  <div diff:ignore></div>
			</div>
			"""";

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.Find("#answered").Click();
		cut.Find("#input-answer").Change("");
		cut.Find("#confirm-answered-status").Click();

		cut.MarkupMatches(_expectedHtml);

	}

	[Fact]
	public void Details_With_AttemptOfCommentAuthorToVote_Should_Fail_Test()
	{

		// Arrange
		_expectedIssue = TestIssues.GetIssues().ToList()[0];
		_expectedComments = TestComments.GetComments().ToList();
		_expectedUser = TestUsers.GetKnownUser();
		_expectedUser.Id = "5dc1039a1521eaa36835e543";
		const string _expectedHtml =
		"""
		<h1 class="page-heading text-light text-uppercase mb-4">Issue Details</h1>
		<div diff:ignore></div>
		<div class="form-layout mb-3">
		  <div diff:ignore></div>
		  <div diff:ignore></div>
		  <div diff:ignore></div>
		  <div class="issue-container">
		    <div class="form-layout comment-details">
		      <div class="fw-bold mb-2">Comments</div>
		      <div id="comment-entry">
		        <div id="vote" class="issue-detail-no-votes"  style="grid-column-start: 1;">
		          <div class="text-uppercase">Awaiting</div>
		          <span class="oi oi-caret-top detail-upvote"></span>
		          <div class="text-uppercase">UpVote</div>
		        </div>
		        <div>
		          <div class="issue-detail-text">Test Comment 1</div>
		          <div class="comment-header">
		            <label class="category-date" diff:ignoreChildren>02.07.2023</label>
		            <label class="category-author">TEST USER</label>
		          </div>
		        </div>
		      </div>
		      <div diff:ignore></div>
		      <div diff:ignore></div>
		    </div>
		  </div>
		</div>
		""";

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.FindAll("#vote")[0].Click();

		// Assert
		cut.MarkupMatches(_expectedHtml);
	}

	[Theory(DisplayName = "Update Status")]
	[InlineData(5, "watching")]
	[InlineData(5, "answered")]
	[InlineData(5, "inwork")]
	[InlineData(5, "dismissed")]
	public void Details_With_WhenStatusIsClicked_Should_ShouldSaveNewStatus_Test(int index, string statusId)
	{

		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssue = TestIssues.GetIssues().ToList()[index];
		_expectedComments = TestComments.GetComments().ToList();

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		IRenderedComponent<Details> cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		switch (statusId)
		{
			case "answered":
				cut.Find("#answered").Click();
				cut.Find("#input-answer").Change("http://localhost/");
				cut.Find("#confirm-answered-status").Click();
				break;
			case "watching":
				cut.Find("#watching").Click();
				cut.Find("#confirm-status-change").Click();
				break;
			case "dismissed":
				cut.Find("#dismissed").Click();
				cut.Find("#confirm-status-change").Click();
				break;
			case "inwork":
				cut.Find("#inwork").Click();
				cut.Find("#confirm-status-change").Click();
				break;
		}

		// Assert
		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssue(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);

	}

	private void SetupMocks()
	{

		_issueRepositoryMock
			.Setup(x => x.GetIssue(_expectedIssue.Id))
			.ReturnsAsync(_expectedIssue);

		_userRepositoryMock
			.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>()))
			.ReturnsAsync(_expectedUser);

		_commentRepositoryMock
			.Setup(x => x.GetCommentsByIssue(It.IsAny<string>()))
			.ReturnsAsync(_expectedComments);

		_expectedStatuses = TestStatuses.GetStatuses().ToList();
		_statusRepositoryMock
			.Setup(x => x.GetStatuses())
			.ReturnsAsync(_expectedStatuses);

	}

	private void SetAuthenticationAndAuthorization(bool isAdmin)
	{

		TestAuthorizationContext authContext = this.AddTestAuthorization();
		authContext.SetAuthorized(_expectedUser.DisplayName);
		authContext.SetClaims(
			new Claim("objectidentifier", _expectedUser.Id)
		);

		if (isAdmin) authContext.SetPolicies("Admin");
	}

	private void RegisterServices()
	{

		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
			_memoryCacheMock.Object));
		Services.AddSingleton<IStatusService>(new StatusService(_statusRepositoryMock.Object,
			_memoryCacheMock.Object));
		Services.AddSingleton<ICommentService>(new CommentService(_commentRepositoryMock.Object,
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