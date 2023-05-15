namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetByUserCommentTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;
	private const string? CleanupValue = "comments";

	public GetByUserCommentTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CommentRepository(context);

	}

	[Fact]
	public async Task GetByUserAsync_With_Valid_Data_Should_Be_Successful_TestAsync()
	{

		// Arrange
		var comment = FakeComment.GetNewComment();
		await _sut.CreateAsync(comment);
		var userId = comment.Author.Id;

		// Act
		var result = (await _sut.GetByUserAsync(userId))!.ToList();

		// Assert
		result.Should().NotBeNull();
		result.Count.Should().Be(1);
		result[0].Author.Should().BeEquivalentTo(comment.Author);

	}

	[Fact]
	public Task InitializeAsync()
	{

		return Task.CompletedTask;

	}

	[Fact]
	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
