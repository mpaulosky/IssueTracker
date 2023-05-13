using IssueTracker.Services.Comment;
using IssueTracker.Services.Comment.Interface;
using IssueTracker.Services.Issue;
using IssueTracker.Services.Issue.Interface;
using IssueTracker.Services.Status;
using IssueTracker.Services.Status.Interface;
using IssueTracker.Services.User;
using IssueTracker.Services.User.Interface;

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
	private readonly IssueModel _expectedIssue;
	private readonly UserModel _expectedUser;
	private readonly List<StatusModel> _expectedStatuses;

	public DetailsTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

		_expectedUser = FakeUser.GetNewUser(true);
		_expectedIssue = FakeIssue.GetNewIssue(true);
		_expectedStatuses = FakeStatus.GetStatuses().ToList();

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
		const string expectedUri = "http://localhost/";

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act
		var cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.Find("#close-page").Click();

		// Assert
		var navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact]
	public void Details_With_NonAdminUser_Should_ShowDetailsNotSetStatus_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-light text-uppercase mb-4">Issue Details</h1>
			<div class="issue-container">
				<button id="create-comment" class="suggest-btn btn btn-outline-light btn-lg text-uppercase">Add Comment</button>
			</div>
			<div class="form-layout mb-3">
				<div class="close-button-section">
					<button id="close-page" class="btn btn-close" ></button>
				</div>
				<div class="issue-container">
					<div class="issue-entry">
						<div diff:ignore></div>
						<div diff:ignore></div>
						<div diff:ignore></div>
					</div>
				</div>
				<div diff:ignore></div>
			</div>
			""";

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act
		var cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Details_With_AdminUser_Should_BeAbleToSetStatus_Test()
	{
		// Arrange
		const string expectedHtml =
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
		        <button id="answered"  class="btn text-category btn-archive btn-status-answered">
		          answered
		        </button>
		        <button id="inwork"  class="btn text-category btn-archive btn-status-inwork">
		          in work
		        </button>
		        <button id="watching"  class="btn text-category btn-archive btn-status-watching">
		          watching
		        </button>
		        <button id="dismissed"  class="btn text-category btn-archive btn-status-dismissed">
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
		var cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Details_With_AddCommentClick_Should_NavigateToCommentPage_Test()
	{
		// Arrange
		SetupMocks();
		SetMemoryCache();

		var expectedUri = $"http://localhost/Comment/{_expectedIssue.Id}";

		SetAuthenticationAndAuthorization(false);
		RegisterServices();

		// Act
		var cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.Find("#create-comment").Click();

		// Assert
		var navMan = Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Theory(DisplayName = "Validate Status Styles")]
	[InlineData(0, "issue-entry-status-answered")]
	[InlineData(1, "issue-entry-status-watching")]
	[InlineData(2, "issue-entry-status-inwork")]
	[InlineData(3, "issue-entry-status-dismissed")]
	[InlineData(4, "issue-entry-status-none")]
	public void Details_With_ValidIssue_Should_ShowStatusStyle_Test(int index, string expected)
	{
		// Arrange
		_expectedIssue.IssueStatus = index == 4 ? new BasicStatusModel() : new BasicStatusModel(_expectedStatuses[index]);

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		var results = cut.FindAll("div");

		var items = results.Select(x => x.ClassName).Where(z => z != null && z.Contains(expected)).ToList();

		// Assert
		items.Count.Should().Be(1);
	}

	[Fact]
	public void Details_When_CommentVotedOnNonAuthor_Should_SaveUpdatedComment_Test()
	{
		// Arrange
		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.FindAll("#vote")[0].Click();

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
	}

	[Fact]
	public void Details_WhenCommentHasVoteByUser_Should_RemoveVote_Test()
	{

		// Arrange
		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.FindAll("#vote")[2].Click();

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

	}

	[Fact]
	public void Details_With_ChangingAnsweredStatusWithoutUrl_Should_Fail_Test()
	{

		// Arrange
		const string expectedHtml =
			""""
			<h1 class="page-heading text-light text-uppercase mb-4">Issue Details</h1>
			<div diff:ignore></div>
			<div class="form-layout mb-3">
			  <div diff:ignore></div>
			  <div class="issue-container">
			    <div class="issue-entry">
			      <div diff:ignore></div>
			      <div diff:ignore></div>
			      <div diff:ignore></div>
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
		var cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.Find("#answered").Click();
		cut.Find("#input-answer").Change("");
		cut.Find("#confirm-answered-status").Click();

		cut.MarkupMatches(expectedHtml);

	}

	[Fact]
	public void Details_With_AttemptOfCommentAuthorToVote_Should_Fail_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-light text-uppercase mb-4">Issue Details</h1>
			<div class="issue-container">
				<button id="create-comment"  class="suggest-btn btn btn-outline-light btn-lg text-uppercase">Add Comment</button>
			</div>
			<div class="form-layout mb-3">
				<div class="close-button-section">
					<button id="close-page" class="btn btn-close" ></button>
				</div>
				<div class="issue-container">
					<div diff:ignore>
					</div>
				</div>
				<div class="issue-container">
					<div diff:ignore>
					</div>
				</div>
				<div diff:ignore></div>
			</div>
			""";

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Details>(parameter =>
		{
			parameter.Add(p => p.Id, _expectedIssue.Id);
		});

		cut.FindAll("#vote")[0].Click();

		// Assert
		cut.MarkupMatches(expectedHtml);

	}

	[Theory(DisplayName = "Update Status")]
	[InlineData("watching")]
	[InlineData("answered")]
	[InlineData("inwork")]
	[InlineData("dismissed")]
	public void Details_With_WhenStatusIsClicked_Should_ShouldSaveNewStatus_Test(string statusId)
	{
		// Arrange
		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Details>(parameter =>
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
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	private void SetupMocks()
	{

		_issueRepositoryMock
			.Setup(x => x.GetAsync(_expectedIssue.Id))
			.ReturnsAsync(_expectedIssue);

		_userRepositoryMock
			.Setup(x => x.GetFromAuthenticationAsync(It.IsAny<string>()))
			.ReturnsAsync(_expectedUser);

		var comments = FakeComment.GetComments(3).ToList();
		comments[1].UserVotes.Add(_expectedUser.Id);
		foreach (var comment in comments)
		{
			comment.CommentOnSource = new BasicCommentOnSourceModel(_expectedIssue);
		}
		_commentRepositoryMock
			.Setup(x => x.GetBySourceAsync(It.IsAny<BasicCommentOnSourceModel>()))
			.ReturnsAsync(comments);

		_statusRepositoryMock
			.Setup(x => x.GetAllAsync())
			.ReturnsAsync(_expectedStatuses);

	}

	private void SetAuthenticationAndAuthorization(bool isAdmin)
	{
		var authContext = this.AddTestAuthorization();
		authContext.SetAuthorized(_expectedUser.DisplayName);
		authContext.SetClaims(
			new Claim("objectidentifier", _expectedUser.Id)
		);

		if (isAdmin)
		{
			authContext.SetPolicies("Admin");
		}
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
