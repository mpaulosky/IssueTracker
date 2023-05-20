﻿using MongoDB.Bson;

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpVoteCommentTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;
	private const string? CleanupValue = "comments";

	public UpVoteCommentTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CommentRepository(context);

	}

	[Fact(DisplayName = "UpVoteAsync With Valid Comment Should Add Vote")]
	public async Task UpVoteAsync_With_ValidComment_Should_AddUserToUpVoteField_Test()
	{

		// Arrange
		var expectedUserId = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
		var expected = FakeComment.GetNewComment();
		// Clear any existing User Votes
		expected.UserVotes.Clear();

		await _sut.CreateAsync(expected);

		// Act
		await _sut.UpVoteAsync(expected.Id, expectedUserId);

		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.UserVotes.Should().Contain(expectedUserId);

	}

	[Fact(DisplayName = "UpVoteAsync With User Already Voted Should Remove User Vote")]
	public async Task UpVoteAsync_With_UserAlreadyVoted_Should_RemoveUsersVote_Test()
	{

		// Arrange
		var expectedUserId = Guid.NewGuid().ToString("N");
		var expected = FakeComment.GetNewComment();

		// Add the User to User Votes
		expected.UserVotes.Add(expectedUserId);

		await _sut.CreateAsync(expected);

		// Act
		await _sut.UpVoteAsync(expected.Id, expectedUserId);

		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.UserVotes.Should().BeEmpty();

	}

	public Task InitializeAsync()
	{

		return Task.CompletedTask;

	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(CleanupValue);

	}
}
