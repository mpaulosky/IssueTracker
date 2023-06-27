// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CommentComponentTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

namespace IssueTracker.UI.Components;

[ExcludeFromCodeCoverage]
public class CommentComponentTests : TestContext
{
	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<ICommentService> _commentServiceMock;
	private readonly CommentModel _expectedComment;
	private readonly UserModel _expectedUser;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	public CommentComponentTests()
	{
		_commentServiceMock = new Mock<ICommentService>();
		_commentRepositoryMock = new Mock<ICommentRepository>();

		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();

		_expectedUser = FakeUser.GetNewUser(true);
		_expectedComment = FakeComment.GetNewComment(true);
	}

	private IRenderedComponent<CommentComponent> ComponentUnderTest()
	{
		SetupMocks();
		SetMemoryCache();
		RegisterServices();

		IRenderedComponent<CommentComponent> component = RenderComponent<CommentComponent>(parameter =>
		{
			parameter.Add(p => p.Item, _expectedComment);
			parameter.Add(p => p.LoggedInUser, _expectedUser);
		});

		return component;
	}

	[Fact(DisplayName = "CommentComponent not Admin")]
	public void CommentComponent_With_NotAdmin_Should_NotDisplaysArchiveButton_Test()
	{
		// Arrange
		const string expected =
			"""
			<div class="comment-item-container">
				<div class="comment-vote comment-no-votes">
					<div id="vote">
						<div>Click To</div>
						<span class="oi oi-caret-top comment-detail-upvote"></span>
						<div>UpVote</div>
					</div>
				</div>
				<div class="comment-entry-text">
					<div diff:ignore></div>
					<div diff:ignore></div>
					<div class="comment-entry-bottom">
						<div diff:ignore></div>
						<div diff:ignore></div>
					</div>
				</div>
				<div class="comment-answer-status comment-answer-status-unanswered" ></div>
			</div>
			""";

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		// Assert 
		cut.MarkupMatches(expected);
	}

	[Fact(DisplayName = "CommentComponent is Admin")]
	public void CommentComponent_With_IsAdmin_Should_DisplaysArchiveButton_Test()
	{
		// Arrange
		const string expected = "archive";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		// Assert 
		cut.Find("#archive").TextContent.Should().Contain(expected);
	}

	[Fact(DisplayName = "CommentComponent is Not Admin and is Not Owner Display nothing")]
	public void CommentComponent_With_IsNotAdminAndNotOwner_Should_DisplaysEmptyArea_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		// Assert 
		cut.Find("div.comment-answer-status").TextContent.Should().BeEmpty();
		cut.MarkupMatches(
			"""
				 <div class="comment-item-container">
					 <div class="comment-vote comment-no-votes">
						 <div id="vote">
							 <div >Click To</div>
							 <span class="oi oi-caret-top comment-detail-upvote"></span>
							 <div >UpVote</div>
						 </div>
					 </div>
					 <div class="comment-entry-text">
						 <div diff:ignore></div>
						 <div diff:ignore></div>
						 <div class="comment-entry-bottom">
							 <div diff:ignore></div>
							 <div diff:ignore></div>
						 </div>
					 </div>
					 <div class="comment-answer-status comment-answer-status-unanswered"></div>
				 </div>
			""");
	}

	[Fact(DisplayName = "CommentComponent is Not Admin and is Not Owner is Answered Display Answered")]
	public void CommentComponent_With_IsNotAdminAndNotOwnerIsAnswered_Should_DisplaysAnswered_Test()
	{
		// Arrange
		const string expected = "Answer";
		SetAuthenticationAndAuthorization(false, true);
		_expectedComment.IsAnswer = true;

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		// Assert 
		cut.Find("div.comment-answer-status").TextContent.Should().Contain(expected);
		cut.MarkupMatches(
			"""
				 <div class="comment-item-container">
					 <div class="comment-vote comment-no-votes">
						 <div id="vote">
							 <div >Click To</div>
							 <span class="oi oi-caret-top comment-detail-upvote"></span>
							 <div >UpVote</div>
						 </div>
					 </div>
					 <div class="comment-entry-text">
						 <div diff:ignore></div>
						 <div diff:ignore></div>
						 <div class="comment-entry-bottom">
							 <div diff:ignore></div>
							 <div diff:ignore></div>
						 </div>
					 </div>
					 <div class="comment-answer-status comment-answer-status-answered">
						 <div id="answered" class="comment-text-status">
							 <span class="oi oi-check"></span>
							 <span >Answer</span>
						 </div>
					 </div>
				 </div>
			""");
	}


	[Fact(DisplayName = "CommentComponent is Admin Display Set Answer Admin")]
	public void CommentComponent_With_IsAdmin_Should_DisplaysSetAnswerButton_Test()
	{
		// Arrange
		const string expected = "set as answer";

		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		// Assert 
		//cut.MarkupMatches("");
		cut.Find("#answer-admin").TextContent.Should().Contain(expected);
	}

	[Fact(DisplayName = "CommentComponent is Admin Display Set Answer Owner")]
	public void CommentComponent_With_IsNotAdminButIsOwner_Should_DisplaysSetAnswerButton_Test()
	{
		// Arrange
		const string expected = "set as answer";
		_expectedComment.Issue!.Author = new BasicUserModel(_expectedUser);

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		// Assert 
		//cut.MarkupMatches("");
		cut.Find("#answer-owner").TextContent.Should().Contain(expected);
	}

	[Fact(DisplayName = "CommentComponent Admin Select Comment as Answer")]
	public void CommentComponent_With_IsAdminSelectAsAnswer_Should_UpdateCommentAsAnswer_Test()
	{
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		// Assert 
		cut.Find("#answer-admin").Click();

		_commentRepositoryMock.Verify(x => x
			.UpdateAsync(It.IsAny<string>(), It.IsAny<CommentModel>()), Times.Once);
	}

	[Fact(DisplayName = "CommentComponent Owner Select Comment as Answer")]
	public void CommentComponent_With_IsOwnerSelectAsAnswer_Should_UpdateCommentAsAnswer_Test()
	{
		SetAuthenticationAndAuthorization(false, true);
		_expectedComment.Issue!.Author = new BasicUserModel(_expectedUser);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		// Assert 
		cut.Find("#answer-owner").Click();

		_commentRepositoryMock.Verify(x => x
			.UpdateAsync(It.IsAny<string>(), It.IsAny<CommentModel>()), Times.Once);
	}

	[Fact]
	public async Task VoteUp_RemovesUserVoteIfAlreadyUpVoted_TestAsync()
	{
		// Arrange
		const int expectedCount = 0;
		_expectedComment.UserVotes.Add(_expectedUser.Id);

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		await cut.Instance.VoteUp(_expectedComment);

		// Assert
		_expectedComment.UserVotes.Count.Should().Be(expectedCount);

		_commentRepositoryMock.Verify(x => x
			.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
	}

	[Fact]
	public async Task VoteUp_AddsUserVoteIfNotAlreadyUpVoted_TestAsync()
	{
		// Arrange
		const int expectedCount = 1;

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		await cut.Instance.VoteUp(_expectedComment);

		// Assert
		_expectedComment.UserVotes.Count.Should().Be(expectedCount);

		_commentRepositoryMock.Verify(x => x
			.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
	}

	[Fact]
	public async Task VoteUp_ReturnsIfAuthorEqualsLoggedInUser_TestAsync()
	{
		// Arrange
		const int expectedCount = 0;
		_expectedComment.Author = new BasicUserModel(_expectedUser);

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		await cut.Instance.VoteUp(_expectedComment);

		// Assert
		_expectedComment.UserVotes.Count.Should().Be(expectedCount);
	}

	[Fact]
	public void GetUpVoteTopText_ReturnsUserVoteCountAsStringWithPadding_TestAsync()
	{
		// Arrange
		const string expectedText = "01";
		_expectedComment.UserVotes.Add(_expectedUser.Id);

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		string result = cut.Instance.GetUpVoteTopText(_expectedComment);

		// Assert 
		result.Should().Be(expectedText);
	}

	[Fact]
	public void GetUpVoteTopText_ReturnsAwaitingIfCurrentUserIsAuthor_TestAsync()
	{
		// Arrange
		const string expectedText = "Awaiting";
		_expectedComment.Author = new BasicUserModel(_expectedUser);

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		string result = cut.Instance.GetUpVoteTopText(_expectedComment);

		// Assert 
		result.Should().Be(expectedText);
	}

	[Fact]
	public void GetUpVoteTopText_ReturnsClickToIfUserHasNotYetVoted_TestAsync()
	{
		// Arrange
		const string expectedText = "Click To";
		_expectedComment.UserVotes.Clear();

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		string result = cut.Instance.GetUpVoteTopText(_expectedComment);

		// Assert 
		result.Should().Be(expectedText);
	}

	[Fact]
	public void GetUpVoteBottomText_ReturnsUpVoteForSingleUpVote_TestAsync()
	{
		// Arrange
		const string expectedText = "UpVote";
		_expectedComment.UserVotes.Clear();

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		string result = cut.Instance.GetUpVoteBottomText(_expectedComment);

		// Assert 
		result.Should().Be(expectedText);
	}

	[Fact]
	public void GetUpVoteBottomText_ReturnsUpVotesForMultipleUpVotes()
	{
		// Arrange
		const string expectedText = "UpVotes";
		_expectedComment.UserVotes.Add(_expectedUser.Id);
		UserModel anotherUser = FakeUser.GetNewUser(true);
		_expectedComment.UserVotes.Add(anotherUser.Id);

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		string result = cut.Instance.GetUpVoteBottomText(_expectedComment);

		// Assert 
		result.Should().Be(expectedText);
	}

	[Fact]
	public void GetVoteCssClass_ReturnsCommentDetailNoVotesIfNoVotes_TestAsync()
	{
		// Arrange
		const string expectedText = "comment-no-votes";
		_expectedComment.UserVotes.Clear();

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		string result = cut.Instance.GetVoteCssClass(_expectedComment);

		// Assert 
		result.Should().Be(expectedText);
	}

	[Fact]
	public void GetVoteCssClass_ReturnsCommentDetailVotedIfCurrentUserHasNotVoted_TestAsync()
	{
		// Arrange
		const string expectedText = "comment-not-voted";
		_expectedComment.UserVotes.Add(_expectedUser.Id);

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		string result = cut.Instance.GetVoteCssClass(_expectedComment);

		// Assert 
		result.Should().Be(expectedText);
	}

	[Fact]
	public void GetVoteCssClass_ReturnsCommentDetailNotVotedIfCurrentUserHasVoted_TestAsync()
	{
		// Arrange
		const string expectedText = "comment-voted";
		_expectedComment.UserVotes.Clear();

		UserModel user = FakeUser.GetNewUser(true);

		_expectedComment.UserVotes.Add(user.Id);

		SetAuthenticationAndAuthorization(false, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		string result = cut.Instance.GetVoteCssClass(_expectedComment);

		// Assert 
		result.Should().Be(expectedText);
	}

	[Fact]
	public void CommentComponent_ArchiveComment_Should_ArchiveTheComment_Test()
	{
		// Arrange
		SetAuthenticationAndAuthorization(true, true);

		// Act
		IRenderedComponent<CommentComponent> cut = ComponentUnderTest();

		cut.Find("#archive").Click();
		cut.Find("#confirm").Click();

		// Assert
		_commentRepositoryMock.Verify(x => x
			.UpdateAsync(It.IsAny<string>(), It.IsAny<CommentModel>()), Times.Once);
	}

	private void SetupMocks()
	{
		_commentRepositoryMock
			.Setup(x => x
				.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()))
			.Returns(Task.CompletedTask).Verifiable();

		_commentServiceMock.Setup(x => x
			.UpVoteComment(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
	}

	private void SetAuthenticationAndAuthorization(bool isAdmin, bool isAuth)
	{
		TestAuthorizationContext authContext = this.AddTestAuthorization();

		if (isAuth)
		{
			authContext.SetAuthorized(_expectedUser.DisplayName);
			authContext.SetClaims(
				new Claim("objectidentifier", _expectedUser.ObjectIdentifier)
			);
		}

		if (isAdmin)
		{
			authContext.SetPolicies("Admin");
		}
	}

	private void RegisterServices()
	{
		Services.AddSingleton<ICommentService>(
			new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object));
	}

	private void SetMemoryCache()
	{
		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);
	}
}