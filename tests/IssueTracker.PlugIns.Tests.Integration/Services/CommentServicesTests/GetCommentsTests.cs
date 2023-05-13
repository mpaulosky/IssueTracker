using IssueTracker.Services.Comment;

namespace IssueTracker.PlugIns.Tests.Integration.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentsTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;
	private string? _cleanupValue;

	public GetCommentsTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		memCache.Remove("CommentsData");


		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task GetComments_With_ValidData_Should_ReturnComments_Test()
	{

		// Arrange
		_cleanupValue = "comments";
		var expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		var results = await _sut.GetComments();

		// Assert
		results.Count.Should().Be(1);
		results[0].Title.Should().Be(expected.Title);
		results[0].Author.Should().BeEquivalentTo(expected.Author);
		results[0].CommentOnSource!.SourceType.Should().Be(expected.CommentOnSource!.SourceType);

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
