using IssueTracker.UI.Pages;

namespace IssueTracker.UI.Tests.Unit.Pages;

[ExcludeFromCodeCoverage]
public class ProfileTests : TestContext
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly List<CommentModel> _expectedComments;
	private readonly List<IssueModel> _expectedIssues;
	private readonly UserModel _expectedUser;

	public ProfileTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

		_expectedUser = FakeUser.GetNewUser(true);
		_expectedIssues = FakeIssue.GetIssues(5).ToList();
		_expectedComments = FakeComment.GetComments(5).ToList();

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

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false, true);
		RegisterServices();

		// Act
		var cut = RenderComponent<Profile>();

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
		foreach (var issue in _expectedIssues)
		{

			issue.Author = new BasicUserModel(_expectedUser);
			issue.ApprovedForRelease = true;
			issue.Archived = false;
			issue.Rejected = false;

		}

		foreach (var comment in _expectedComments)
		{

			comment.Author = new BasicUserModel(_expectedUser);
			comment.Archived = false;

		}

		SetupMocks();
		SetMemoryCache();

		SetAuthenticationAndAuthorization(false, true);
		RegisterServices();

		// Act
		IRenderedComponent<Profile> cut = RenderComponent<Profile>();
		var issueDivs = cut.FindAll("div.issue-container").ToList();
		var commentDivs = cut.FindAll("div#comment-entry").ToList();

		// Assert
		issueDivs.Count.Should().Be(5);
		commentDivs.Count.Should().Be(5);

	}

	private void SetupMocks()
	{
		_issueRepositoryMock
			.Setup(x => x.GetByUserAsync(_expectedUser.Id))
			.ReturnsAsync(_expectedIssues);

		_userRepositoryMock
			.Setup(x => x.GetFromAuthenticationAsync(It.IsAny<string>()))
			.ReturnsAsync(_expectedUser);

		_commentRepositoryMock
			.Setup(x => x.GetByUserAsync(It.IsAny<string>())).ReturnsAsync(_expectedComments);
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		TestAuthorizationContext authContext = this.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName);
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.Id)
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
