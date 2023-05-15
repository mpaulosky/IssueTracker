using MongoDB.Bson;

namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpVoteSolutionTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly SolutionRepository _sut;
	private const string? CleanupValue = "solutions";

	public UpVoteSolutionTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new SolutionRepository(context);

	}

	[Fact(DisplayName = "UpVoteAsync With Valid Solution Should Add Vote")]
	public async Task UpVoteAsync_With_ValidSolution_Should_AddUserToUpVoteField_Test()
	{

		// Arrange
		var expectedUserId = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
		var expected = FakeSolution.GetNewSolution();
		// Clear any existing User Votes
		expected.UserVotes.Clear();

		await _sut.CreateAsync(expected);

		// Act
		await _sut.UpVoteAsync(expected.Id, expectedUserId);

		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result!.UserVotes.Should().Contain(expectedUserId);

	}

	[Fact(DisplayName = "UpVoteAsync With User Already Voted Should Remove User Vote")]
	public async Task UpVoteAsync_With_UserAlreadyVoted_Should_RemoveUsersVote_Test()
	{

		// Arrange
		var expectedUserId = Guid.NewGuid().ToString("N");
		var expected = FakeSolution.GetNewSolution();

		// Add the User to User Votes
		expected.UserVotes.Add(expectedUserId);

		await _sut.CreateAsync(expected);

		// Act
		await _sut.UpVoteAsync(expected.Id, expectedUserId);

		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result!.UserVotes.Should().BeEmpty();

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
