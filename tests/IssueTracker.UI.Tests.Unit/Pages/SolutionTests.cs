﻿namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class SolutionTests : TestContext
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly List<CommentModel> _expectedComments;
	private readonly List<IssueModel> _expectedIssues;
	private readonly UserModel _expectedUser;


	public SolutionTests()
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

	private IRenderedComponent<Solution> ComponentUnderTest()
	{

		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		IRenderedComponent<Solution> component = RenderComponent<Solution>();

		return component;

	}

	[Fact(DisplayName = "Check Logged In User")]
	public Task CheckLoggedInUser_With_NullLoggedInUser_Should_ThrowAArgumentNullException_TestAsync()
	{

		// Arrange
		const string expectedParamName = "userObjectIdentifierId";
		const string expectedMessage = "Value cannot be null.?*";

		SetAuthenticationAndAuthorization(false, false);

		// Act
		Func<IRenderedComponent<Solution>> cut = ComponentUnderTest;

		// Assert
		cut.Should()
			.Throw<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
		return Task.CompletedTask;
	}

	private void SetupMocks()
	{

		_issueRepositoryMock
			.Setup(x => x
				.GetByUserAsync(_expectedUser.Id))
			.ReturnsAsync(_expectedIssues);

		_userRepositoryMock
			.Setup(x => x
				.GetFromAuthenticationAsync(It.IsAny<string>()))
			.ReturnsAsync(_expectedUser);

		_commentRepositoryMock
			.Setup(x => x
				.GetByUserAsync(It.IsAny<string>()))
			.ReturnsAsync(_expectedComments);

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

		Services.AddSingleton<IIssueService>(
			new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));

		Services.AddSingleton<ICommentService>(
			new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object));

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
