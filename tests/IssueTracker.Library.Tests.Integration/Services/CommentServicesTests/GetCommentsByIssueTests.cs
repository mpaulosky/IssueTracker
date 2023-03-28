namespace IssueTracker.PlugIns.Mongo.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentsByIssueTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;
	private string _cleanupValue;

	public GetCommentsByIssueTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		memCache.Remove("CommentsData");

		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task GetCommentsByIssue_With_ValidData_Should_ReturnValidComment_Test()
	{
		// Arrange
		_cleanupValue = "comments";
		CommentModel expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		List<CommentModel> result = await _sut.GetCommentsByIssue(expected.Source.Id);

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result[0].Source.Id.Should().Be(expected.Source.Id);

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}
}