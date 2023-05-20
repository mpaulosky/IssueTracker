namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetBySourceCommentTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;
	private const string CleanupValue = "comments";

	public GetBySourceCommentTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CommentRepository(context);

	}

	[Fact]
	public async Task GetBySourceAsync_With_Valid_Data_Should_Be_Successful_TestAsync()
	{
		// Arrange
		var comments = FakeComment.GetComments(3).ToList();
		var source = comments[0].CommentOnSource!;
		foreach (var comment in comments)
		{
			comment.Id = string.Empty;
			comment.CommentOnSource!.Id = source.Id;
			comment.CommentOnSource.SourceType = source.SourceType;
			await _sut.CreateAsync(comment);
		}

		// Act
		var result = (await _sut.GetBySourceAsync(source))!.ToList();

		// Assert
		result.Should().NotBeNull();
		result.Count.Should().Be(1);
		result[0].CommentOnSource!.Id.Should().Be(source.Id);
		result[0].CommentOnSource!.SourceType.Should().Be(source.SourceType);

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
