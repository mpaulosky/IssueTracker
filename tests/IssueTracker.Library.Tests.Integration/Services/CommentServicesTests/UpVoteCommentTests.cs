using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
using IssueTracker.PlugIns.Services;

namespace IssueTracker.PlugIns.Mongo.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpVoteCommentTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;
	private string _cleanupValue;

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
		_cleanupValue = "comments";
		var expectedUserId = Guid.NewGuid().ToString("N");
		CommentModel expected = FakeComment.GetNewComment();
		// Clear any existing User Votes
		expected.UserVotes.Clear();

		await _sut.CreateComment(expected);

		// Act
		await _sut.UpVoteComment(expected.Id, expectedUserId);

		CommentModel result = await _sut.GetComment(expected.Id);

		// Assert
		result.UserVotes.Should().Contain(expectedUserId);

	}

	[Fact]
	public async Task UpVoteComment_With_UserAlreadyVoted_Should_RemoveUsersVote_Test()
	{

		// Arrange
		_cleanupValue = "comments";
		var expectedUserId = Guid.NewGuid().ToString("N");
		CommentModel expected = FakeComment.GetNewComment();

		// Add the User to User Votes
		expected.UserVotes.Add(expectedUserId);

		await _sut.CreateComment(expected);

		// Act
		await _sut.UpVoteComment(expected.Id, expectedUserId);

		CommentModel result = await _sut.GetComment(expected.Id);

		// Assert
		result.UserVotes.Should().BeEmpty();

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(_cleanupValue);

	}
}
