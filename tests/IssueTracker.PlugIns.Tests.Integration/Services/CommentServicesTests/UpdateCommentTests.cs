namespace IssueTracker.PlugIns.Tests.Integration.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateCommentTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;
	private string? _cleanupValue;

	public UpdateCommentTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task UpdateComment_With_ValidData_Should_UpdateTheComment_Test()
	{

		// Arrange
		_cleanupValue = "comments";
		var expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		expected.Title = "Updated";
		await _sut.UpdateComment(expected);
		var result = await _sut.GetComment(expected.Id);

		// Assert
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Author.Should().BeEquivalentTo(expected.Author);
		result.CommentOnSource!.SourceType.Should().Be(expected.CommentOnSource!.SourceType);

	}

	[Fact]
	public async Task UpdateComment_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";

		// Act
		Func<Task> act = async () => await _sut.UpdateComment(null!);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

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
