namespace IssueTracker.Library.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class UpVoteCommentTests : IClassFixture<IssueTrackerTestFactory>
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;

	public UpVoteCommentTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;

		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		memCache.Remove("CommentsData");

		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task UpVoteComment_With_ValidComment_Should_AddUserToUpVoteField_Test()
	{

		// Arrange
		var expectedUserId = Guid.NewGuid().ToString("N");
		var expected = FakeComment.GetNewComment();
		// Clear any existing User Votes
		expected.UserVotes.Clear();

		await _sut.CreateComment(expected);

		// Act
		await _sut.UpVoteComment(expected.Id, expectedUserId);

		var result = await _sut.GetComment(expected.Id);

		// Assert
		result.UserVotes.Should().Contain(expectedUserId);

	}

	[Fact]
	public async Task UpVoteComment_With_UserAlreadyVoted_Should_RemoveUsersVote_Test()
	{

		// Arrange
		var expectedUserId = Guid.NewGuid().ToString("N");
		var expected = FakeComment.GetNewComment();

		// Add the User to User Votes
		expected.UserVotes.Add(expectedUserId);

		await _sut.CreateComment(expected);

		// Act
		await _sut.UpVoteComment(expected.Id, expectedUserId);

		var result = await _sut.GetComment(expected.Id);

		// Assert
		result.UserVotes.Should().BeEmpty();

	}

}
