using IssueTracker.PlugIns.PlugInRepositoryInterfaces;

namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class ProfileTests : TestContext
{
	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private List<CommentModel> _expectedComments;
	private List<IssueModel> _expectedIssues;
	private UserModel _expectedUser;

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
		this.AddTestAuthorization();

		RegisterServices();

		// Act

		// Assert
		Assert.Throws<ArgumentNullException>(() => RenderComponent<Profile>()).Message.Should()
			.Be("Value cannot be null. (Parameter 'userObjectIdentifierId')");
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

		SetAuthenticationAndAuthorization(false, true);
		RegisterServices();

		// Act
		IRenderedComponent<Profile> cut = RenderComponent<Profile>();

		cut.Find("#close-page").Click();

		// Assert
		FakeNavigationManager navMan = Services.GetRequiredService<FakeNavigationManager>();
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
		const string _expectedHtml =
			"""
			<h1 class="page-heading text-light text-uppercase mb-4">jim test Profile</h1>
			<div class="form-layout mb-3">
				<div class="close-button-section">
					<button id="close-page" class="btn btn-close" ></button>
				</div>
				<div class="form-layout mb-3">
					<h2 class="my-issue-heading">jim test Account</h2>
					<p class="my-issue-text">
						<a href="MicrosoftIdentity/Account/EditProfile">Edit My Profile</a>
					</p>
				</div>
			</div>
			<div class="form-layout mb-3">
				<h2 class="my-issue-heading">Approved Issues</h2>
				<p class="my-issue-text">These are your issues that are currently available for comment.</p>
				<div diff:ignore></div>
			</div>
			<div class="form-layout mb-3">
				<h2 class="my-issue-heading">Pending Issues</h2>
				<p class="my-issue-text">These are your issues that are currently under review by admin.</p>
				<div diff:ignore></div>
				<div diff:ignore></div>
				<div diff:ignore></div>
			</div>
			<div class="form-layout mb-3">
				<h2 class="my-issue-heading">Rejected Issues</h2>
				<p>These are your issues that were rejected by the admin for being out of the scope of this application.</p>
				<div diff:ignore></div>
			</div>
			<div class="form-layout mb-3">
				<h2 class="my-issue-heading">Archived Issues</h2>
				<p>These are your issues that are archived for future review.</p>
				<div diff:ignore></div>
			</div>
			<div class="form-layout mb-3">
				<h2 class="my-issue-heading">Comments</h2>
				<p class="my-issue-text">These are your comments that are currently active.</p>
				<div diff:ignore></div>
				<div diff:ignore></div>
				<div diff:ignore></div>
			</div>
			""";

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false, true);
		RegisterServices();

		// Act
		IRenderedComponent<Profile> cut = RenderComponent<Profile>();

		// Assert
		cut.MarkupMatches(_expectedHtml);

	}

	private void SetupMocks()
	{
		_issueRepositoryMock
			.Setup(x => x.GetIssuesByUserAsync(_expectedUser.Id))
			.ReturnsAsync(_expectedIssues);

		_userRepositoryMock
			.Setup(x => x.GetUserFromAuthenticationAsync(It.IsAny<string>()))
			.ReturnsAsync(_expectedUser);

		_commentRepositoryMock
			.Setup(x => x.GetCommentsByUserAsync(It.IsAny<string>()))
			.ReturnsAsync(_expectedComments);
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		TestAuthorizationContext authContext = this.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName!);
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.Id!)
			);
		}

		if (isAdmin) authContext.SetPolicies("Admin");
	}

	private void RegisterServices()
	{
		Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object,
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
