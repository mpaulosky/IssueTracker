namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class ProfileTests
{
	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private List<IssueModel> _expectedIssues;
	private UserModel _expectedUser;
	private List<CommentModel> _expectedComments;

	public ProfileTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}

	[Fact]
	public void Profile_With_NullLoggedInUser_Should_ThrowArgumentNullException_Test()
	{
		// Arrange
		using var ctx = new TestContext();

		ctx.AddTestAuthorization();

		RegisterServices(ctx);

		// Act

		// Assert
		Assert.Throws<ArgumentNullException>(() => ctx.RenderComponent<Profile>()).Message.Should().Be("Value cannot be null. (Parameter 'userId')");

	}

	[Fact]
	public void Profile_With_ClosePageClick_Should_NavigateToTheIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssues = TestIssues.GetIssues().ToList();
		_expectedComments = TestComments.GetComments().ToList();

		SetupMocks();
		SetMemoryCache();

		using var ctx = new TestContext();

		SetAuthenticationAndAuthorization(ctx, false, true);
		RegisterServices(ctx);

		// Act
		var cut = ctx.RenderComponent<Profile>();

		cut.Find("#close-page").Click();


		// Assert
		var navMan = ctx.Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);

	}

	[Fact]
	public void Profile_With_ValidIssuesAndComments_Should_DisplayTheIssuesAndComments_Test()
	{
		// Arrange
		_expectedUser = TestUsers.GetKnownUser();
		_expectedIssues = TestIssues.GetIssues().ToList();
		_expectedComments = TestComments.GetComments().ToList();

		SetupMocks();
		SetMemoryCache();

		using var ctx = new TestContext();

		SetAuthenticationAndAuthorization(ctx, false, true);
		RegisterServices(ctx);

		// Act
		var cut = ctx.RenderComponent<Profile>();

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-light text-uppercase mb-4"">jim test Profile</h1>
				<div class=""form-layout mb-3"">
				  <div class=""close-button-section"">
				    <button id=""close-page"" class=""btn btn-close"" ></button>
				  </div>
				  <div class=""form-layout mb-3"">
				    <h2 class=""my-issue-heading"">jim test Account</h2>
				    <p class=""my-issue-text"">
				      <a href=""MicrosoftIdentity/Account/EditProfile"">Edit My Profile</a>
				    </p>
				  </div>
				</div>
				<div class=""form-layout mb-3"">
				  <h2 class=""my-issue-heading"">Approved Issues</h2>
				  <p class=""my-issue-text"">These are your issues that are currently available for comment.</p>
				  <hr>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 6</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 6</p>
				  <div class=""issue-profile-status issue-profile-status-none""></div>
				  <p class=""my-ownernote-text"">Notes for Issue 1</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 3</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 3</p>
				  <div class=""issue-profile-status issue-profile-status-watching"">Watching</div>
				  <p class=""my-ownernote-text"">Notes for Issue 3</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 3</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 3</p>
				  <div class=""issue-profile-status issue-profile-status-dismissed"">Dismissed</div>
				  <p class=""my-ownernote-text"">Notes for Issue 3</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 3</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 3</p>
				  <div class=""issue-profile-status issue-profile-status-inwork"">In Work</div>
				  <p class=""my-ownernote-text"">Notes for Issue 3</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 2</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 2</p>
				  <div class=""issue-profile-status issue-profile-status-answered"">Answered</div>
				  <p class=""my-ownernote-text"">Notes for Issue 2</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 1</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 1</p>
				  <div class=""issue-profile-status issue-profile-status-watching"">Watching</div>
				  <p class=""my-ownernote-text"">Notes for Issue 1</p>
				</div>
				<div class=""form-layout mb-3"">
				  <h2 class=""my-issue-heading"">Pending Issues</h2>
				  <p class=""my-issue-text"">These are your issues that are currently under review by admin.</p>
				  <hr>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 6</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 6</p>
				  <div class=""issue-profile-status issue-profile-status-none""></div>
				  <p class=""my-ownernote-text"">Notes for Issue 1</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 3</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 3</p>
				  <div class=""issue-profile-status issue-profile-status-watching"">Watching</div>
				  <p class=""my-ownernote-text"">Notes for Issue 3</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 3</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 3</p>
				  <div class=""issue-profile-status issue-profile-status-dismissed"">Dismissed</div>
				  <p class=""my-ownernote-text"">Notes for Issue 3</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 3</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 3</p>
				  <div class=""issue-profile-status issue-profile-status-inwork"">In Work</div>
				  <p class=""my-ownernote-text"">Notes for Issue 3</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 2</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 2</p>
				  <div class=""issue-profile-status issue-profile-status-answered"">Answered</div>
				  <p class=""my-ownernote-text"">Notes for Issue 2</p>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 1</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">A new test issue 1</p>
				  <div class=""issue-profile-status issue-profile-status-watching"">Watching</div>
				  <p class=""my-ownernote-text"">Notes for Issue 1</p>
				</div>
				<div class=""form-layout mb-3"">
				  <h2 class=""my-issue-heading"">Rejected Issues</h2>
				  <p>These are your issues that were rejected by the admin for being out of the scope of this application.</p>
				  <hr>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 3</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p>A new test issue 3</p>
				  <div class=""issue-profile-status issue-profile-status-inwork"">In Work</div>
				  <p class=""my-ownernote-text"">Notes for Issue 3</p>
				</div>
				<div class=""form-layout mb-3"">
				  <h2 class=""my-issue-heading"">Archived Issues</h2>
				  <p>These are your issues that are archived for future review.</p>
				  <hr>
				  <hr class=""my-issue-separator"">
				  <div>Test Issue 3</div>
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p>A new test issue 3</p>
				  <div class=""issue-profile-status issue-profile-status-inwork"">In Work</div>
				  <p class=""my-ownernote-text"">Notes for Issue 3</p>
				</div>
				<div class=""form-layout mb-3"">
				  <h2 class=""my-issue-heading"">Comments</h2>
				  <p class=""my-issue-text"">These are your comments that are currently active.</p>
				  <hr>
				  <hr class=""my-issue-separator"">
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">Test Comment 1</p>
				  <hr class=""my-issue-separator"">
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">Test Comment 2</p>
				  <hr class=""my-issue-separator"">
				  <p diff:ignoreChildren diff:ignoreAttributes></p>
				  <p class=""my-issue-text"">Test Comment 3</p>
				</div>"
			);
	}

	private void SetupMocks()
	{
		_issueRepositoryMock
			.Setup(x => x.GetUsersIssues(_expectedUser.Id))
			.ReturnsAsync(_expectedIssues);

		_userRepositoryMock
			.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>()))
			.ReturnsAsync(_expectedUser);

		_commentRepositoryMock
			.Setup(x => x.GetUsersComments(It.IsAny<string>()))
			.ReturnsAsync(_expectedComments);
	}

	private void SetAuthenticationAndAuthorization(TestContext ctx, bool isAdmin, bool isAuth)
	{
		var authContext = ctx.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName);
			authContext.SetClaims(
				new Claim(type: "objectidentifier", _expectedUser.Id)
			);
		}

		if (isAdmin)
		{
			authContext.SetPolicies("Admin");
		}
	}

	private void RegisterServices(TestContext ctx)
	{
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<ICommentService>(new CommentService(_commentRepositoryMock.Object,
			_memoryCacheMock.Object));
		ctx.Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}