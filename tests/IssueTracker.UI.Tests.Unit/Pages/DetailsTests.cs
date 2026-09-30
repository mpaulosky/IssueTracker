// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     DetailsTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

using AngleSharp.Dom;

namespace IssueTracker.UI.Pages;

[ExcludeFromCodeCoverage]
public class DetailsTests : BunitContext
{
	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly IssueModel _expectedIssue;
	private readonly List<StatusModel> _expectedStatuses;
	private readonly UserModel _expectedUser;
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private readonly Mock<IStatusRepository> _statusRepositoryMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;

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
		_expectedIssue.ApprovedForRelease = true;
		_expectedIssue.Rejected = false;
		_expectedStatuses = FakeStatus.GetStatuses().ToList();
	}

	private IRenderedComponent<Details> ComponentUnderTest(string? issueId)
	{
		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		IRenderedComponent<Details> component = Render<Details>(parameter =>
		{
			parameter.Add(p => p.Id, issueId);
		});

		return component;
	}

	[Fact]
	public void Comment_With_NullLoggedInUser_Should_ThrowArgumentNullException_Test()
	{
		// Arrange
		const string expectedParamName = "userObjectIdentifierId";
		const string expectedMessage = "Value cannot be null.?*";

		SetAuthenticationAndAuthorization(false, false);

		// Act
		Func<IRenderedComponent<Details>> cut = () => ComponentUnderTest(_expectedIssue.Id);

		// Assert
		cut.Should()
			.Throw<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact]
	public void Details_WithOut_IssueId_Should_ThrowArgumentNullExceptionOnInitialization_Test()
	{
		// Arrange
		const string expectedParamName = "Id";
		const string expectedMessage = "Value cannot be null.?*";

		SetAuthenticationAndAuthorization(false, true);

		// Act
		Func<IRenderedComponent<Details>> cut = () => ComponentUnderTest(null);

		// Assert
		cut.Should()
			.Throw<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Details_With_Pending_Or_Rejected_Issue_And_Other_User_Should_Show_Not_Found_Test(bool rejected)
	{
		// Arrange
		_expectedIssue.ApprovedForRelease = false;
		_expectedIssue.Rejected = rejected;
		_expectedIssue.Author = new BasicUserModel(FakeUser.GetNewUser(true, true));

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

		// Assert
		cut.Find("#issue-not-found").TextContent.Should().Be("Sorry, there's nothing at this address.");
		cut.FindAll("#create-comment").Should().BeEmpty();
		_commentRepositoryMock
			.Verify(x =>
				x.GetByIssueAsync(It.IsAny<BasicIssueModel>()), Times.Never);
	}

	[Fact]
	public void Details_With_Pending_Issue_And_Admin_Should_Show_Issue_Test()
	{
		// Arrange
		_expectedIssue.ApprovedForRelease = false;
		_expectedIssue.Rejected = false;
		_expectedIssue.Author = new BasicUserModel(FakeUser.GetNewUser(true, true));

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

		// Assert
		cut.FindAll("#issue-not-found").Should().BeEmpty();
		cut.FindAll("#create-comment").Should().ContainSingle();
	}

	[Fact]
	public void Details_ClosePageClick_Should_NavigateToIndexPage_Test()
	{
		// Arrange
		const string expectedUri = "http://localhost/";

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

		cut.Find("#close-page").Click();

		// Assert
		NavigationManager navMan = Services.GetRequiredService<NavigationManager>();
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
			<div diff:ignore></div>
			<div class="form-layout mb-3">
				<div class="close-button-section">
					<button id="close-page" class="btn btn-close"></button>
				</div>
				<div diff:ignore></div>
				<div diff:ignore></div>
			</div>
			""";

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

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
				<div class="issue-item-container">
					<div diff:ignore></div>
					<div diff:ignore></div>
					<div diff:ignore></div>
				</div>
				<div class="issue-container">
					<div class="status-layout flex-container">
						<button class="btn btn-status btn-status-status fw-bold">Set Status</button>
						<button id="accepted" class="btn btn-status btn-status-accepted">
							accepted
						</button>
						<button id="upcoming" class="btn btn-status btn-status-upcoming">
							upcoming
						</button>
						<button id="watching" class="btn btn-status btn-status-watching">
							watching
						</button>
						<button id="dismissed" class="btn btn-status btn-status-dismissed">
							dismissed
						</button>
					</div>
				</div>
				<div diff:ignore></div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	[Fact]
	public void Details_With_AddCommentClick_Should_NavigateToCommentPage_Test()
	{
		// Arrange
		string expectedUri = $"http://localhost/Comment/{_expectedIssue.Id}";

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

		cut.Find("#create-comment").Click();

		// Assert
		NavigationManager navMan = Services.GetRequiredService<NavigationManager>();
		navMan.Uri.Should().NotBeNull();
		navMan.Uri.Should().Be(expectedUri);
	}

	[Theory(DisplayName = "Validate Status Styles")]
	[InlineData(0, "issue-entry-status-accepted")]
	[InlineData(1, "issue-entry-status-watching")]
	[InlineData(2, "issue-entry-status-upcoming")]
	[InlineData(3, "issue-entry-status-dismissed")]
	[InlineData(4, "issue-entry-status-none")]
	public void Details_With_ValidIssue_Should_ShowStatusStyle_Test(int index, string expected)
	{
		// Arrange
		const int expectedCount = 1;
		_expectedIssue.IssueStatus = index == 4 ? new BasicStatusModel() : new BasicStatusModel(_expectedStatuses[index]);

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

		var results = cut.FindAll("div");

		List<string?> items = results.Select(x => x.ClassName).Where(z => z != null && z.Contains(expected)).ToList();

		// Assert
		items.Count.Should().Be(expectedCount);
	}

	[Fact]
	public void Details_When_CommentVotedOnNonAuthor_Should_SaveUpdatedComment_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

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
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

		cut.FindAll("#vote")[2].Click();

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
	}

	[Fact]
	public void Details_With_AttemptOfCommentAuthorToVote_Should_Fail_Test()
	{
		// Arrange
		const string expectedHtml =
			"""
			<h1 class="page-heading text-light text-uppercase mb-4">Issue Details</h1>
			<div diff:ignore></div>
			<div class="form-layout mb-3">
				<div class="close-button-section">
					<button id="close-page" class="btn btn-close"></button>
				</div>
				<div diff:ignore></div>
				<div diff:ignore></div>
				<div class="issue-container">
					<div class="fw-bold mb-2">Comments</div>
					<div class="comment-item-container">
						<div class="comment-vote comment-not-voted">
							<div id="vote">
								<div>01</div>
								<span class="oi oi-caret-top comment-detail-upvote"></span>
								<div>UpVote</div>
							</div>
						</div>
						<div diff:ignore></div>
						<div diff:ignore></div>
					</div>
					<div class="comment-item-container">
						<div class="comment-vote comment-not-voted">
							<div id="vote">
								<div>01</div>
								<span class="oi oi-caret-top comment-detail-upvote"></span>
								<div>UpVote</div>
							</div>
						</div>
						<div diff:ignore></div>
						<div diff:ignore></div>
					</div>
					<div class="comment-item-container">
						<div class="comment-vote comment-no-votes">
							<div id="vote">
								<div>Click To</div>
								<span class="oi oi-caret-top comment-detail-upvote"></span>
								<div>UpVote</div>
							</div>
						</div>
						<div diff:ignore></div>
						<div diff:ignore></div>
					</div>
				</div>
			</div>
			""";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

		cut.FindAll("#vote")[0].Click();

		// Assert
		cut.MarkupMatches(expectedHtml);
	}

	[Theory(DisplayName = "Update Status")]
	[InlineData("watching")]
	[InlineData("accepted")]
	[InlineData("upcoming")]
	[InlineData("dismissed")]
	public void Details_With_WhenStatusIsClicked_Should_ShouldSaveNewStatus_Test(string statusId)
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<Details> cut = ComponentUnderTest(_expectedIssue.Id);

		switch (statusId)
		{
			case "accepted":
				cut.Find("#accepted").Click();
				cut.Find("#confirm-status-change").Click();
				break;
			case "watching":
				cut.Find("#watching").Click();
				cut.Find("#confirm-status-change").Click();
				break;
			case "dismissed":
				cut.Find("#dismissed").Click();
				cut.Find("#confirm-status-change").Click();
				break;
			case "upcoming":
				cut.Find("#upcoming").Click();
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

		List<CommentModel> comments = FakeComment.GetComments(3).ToList();
		comments[1].UserVotes.Add(_expectedUser.Id);
		foreach (CommentModel? comment in comments)
		{
			comment.Issue = new BasicIssueModel(_expectedIssue);
		}

		_commentRepositoryMock
			.Setup(x => x.GetByIssueAsync(It.IsAny<BasicIssueModel>()))
			.ReturnsAsync(comments);

		_statusRepositoryMock
			.Setup(x => x.GetAllAsync())
			.ReturnsAsync(_expectedStatuses);
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		var authContext = AddAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName);
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.Id)
			);
		}

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