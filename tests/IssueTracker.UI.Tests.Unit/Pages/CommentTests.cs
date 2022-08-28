using System.Diagnostics;

namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class CommentTests
{
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	public CommentTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_userRepositoryMock = new Mock<IUserRepository>();
		
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}
	
	[Fact]
	public void Comment_CloseButton_Should_WhenClickedNavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";
		
		var expected = TestIssues.GetKnownIssue();
		_issueRepositoryMock.Setup(x => x.GetIssue(expected.Id)).ReturnsAsync(expected);

		var expectedUser = TestUsers.GetKnownUser();

		_userRepositoryMock.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>())).ReturnsAsync(expectedUser);

		SetMemoryCache();

		using var ctx = new TestContext();
		
		// Set Authentication and Authorization
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized(expectedUser.DisplayName);
		authContext.SetClaims(
			new Claim(type:"objectidentifier", expectedUser.Id)
		);

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<ICommentService>(new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));
		
		// Act
		var cut = ctx.RenderComponent<Comment>((parameter =>
		{
			parameter
				.Add(p => p.Id, expected.Id);
		}));
		
		var buttonElements = cut.FindAll("button");
		buttonElements[0].Click();

		// Assert
		var navMan = ctx.Services.GetRequiredService<FakeNavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Fact()]
	public void Comment_With_Issue_Should_ShowCommentForm_Test()
	{
		// Arrange
		var expected = TestIssues.GetKnownIssue();
		_issueRepositoryMock.Setup(x => x.GetIssue(expected.Id)).ReturnsAsync(expected);

		var expectedUser = TestUsers.GetKnownUser();

		_userRepositoryMock.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>())).ReturnsAsync(expectedUser);

		SetMemoryCache();

		using var ctx = new TestContext();
		
		// Set Authentication and Authorization
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized(expectedUser.DisplayName);
		authContext.SetClaims(
			new Claim(type:"objectidentifier", expectedUser.Id)
		);

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<ICommentService>(new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));

		// Act
		var cut = ctx.RenderComponent<Comment>((parameter =>
		{
			parameter
				.Add(p => p.Id, expected.Id);
		}));

		// Assert
		cut.MarkupMatches
		(
			@"<h1 class=""page-heading text-light text-uppercase mb-4"">Comment on an Issue</h1>
				<div class=""row justify-content-center create-form"">
				  <div class=""col-xl-8 col-lg-10 form-layout"">
				    <div class=""row issue-detail-row"">
				      <div class=""col-11 issue-detail"">
				        <div>
				          <div class=""issue-detail-date"">
				            <div>08.27.2022</div>
				          </div>
				        </div>
				        <div class=""issue-detail-text"">
				          <div class=""fw-bold mb-2 issue-detail-issue"">Test Issue 1</div>
				          <div class=""mb-2 suggestion-detail-author"">Tester</div>
				          <div class=""mb-2 d-none d-md-block"">A new test issue 1</div>
				          <div class=""suggestion-entry-text-category d-none d-md-block"">Miscellaneous</div>
				        </div>
				      </div>
				      <div class=""col-1 close-button-section"">
				        <button class=""btn btn-close"" ></button>
				      </div>
				    </div>
				    <div class=""row d-block d-md-none"">
				      <div class=""issue-detail-text"">
				        <div>A new test issue 1</div>
				        <div class=""issue-entry-text-category"">Miscellaneous</div>
				      </div>
				    </div>
				    <form >
				      <div class=""input-section"">
				        <label class=""form-label fw-bold text-uppercase"" for=""comment"">Comment On the Issue</label>
				        <div class=""input-description"">Describe your suggested solution to the issue.</div>
				        <textarea id=""comment"" class=""form-control valid""  ></textarea>
				      </div>
				      <div class=""center-children"">
				        <button class=""btn btn-main btn-lg text-uppercase"" type=""submit"">Creat Comment</button>
				      </div>
				    </form>
				  </div>
				</div>"
		);
	}

	[Fact()]
	public void Comment_With_ValidComment_Should_SaveTheComment_Test()
	{
		// Arrange
		var expected = TestIssues.GetKnownIssue();
		_issueRepositoryMock.Setup(x => x.GetIssue(expected.Id)).ReturnsAsync(expected);

		UserModel expectedUser = TestUsers.GetKnownUser();

		_userRepositoryMock.Setup(x => x.GetUserFromAuthentication(It.IsAny<string>())).ReturnsAsync(expectedUser);

		SetMemoryCache();

		using var ctx = new TestContext();
		
		// Set Authentication and Authorization
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized(expectedUser.DisplayName);
		authContext.SetClaims(
			new Claim(type:"objectidentifier", expectedUser.Id)
		);

		// Register services
		ctx.Services.AddSingleton<IIssueService>(new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<ICommentService>(new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object));
		ctx.Services.AddSingleton<IUserService>(new UserService(_userRepositoryMock.Object));

		// Act
		var cut = ctx.RenderComponent<Comment>((parameter =>
		{
			parameter
				.Add(p => p.Id, expected.Id);
		}));

		cut.Find("#comment").Change("Test");
		cut.FindAll("button")[1].Click();

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.CreateComment(It.IsAny<CommentModel>()), Times.Once);
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}