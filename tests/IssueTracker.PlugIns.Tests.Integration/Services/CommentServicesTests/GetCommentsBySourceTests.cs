namespace IssueTracker.PlugIns.Tests.Integration.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentsBySourceTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;
	private const string? CleanupValue = "comments";

	public GetCommentsBySourceTests(IssueTrackerTestFactory factory)
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
		var expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		var result = await _sut.GetCommentsBySource(expected.CommentOnSource!);

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result[0].CommentOnSource!.Id.Should().Be(expected.CommentOnSource!.Id);

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
