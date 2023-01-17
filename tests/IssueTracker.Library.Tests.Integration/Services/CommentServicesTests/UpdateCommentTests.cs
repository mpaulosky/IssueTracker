namespace IssueTracker.Library.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateCommentTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;
	private string _cleanupValue;

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
		CommentModel expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		expected.Comment = "Updated";
		await _sut.UpdateComment(expected);
		CommentModel result = await _sut.GetComment(expected!.Id!);

		// Assert
		result.Id.Should().Be(expected!.Id);
		result.Comment.Should().Be(expected!.Comment);
		result.Author.Should().BeEquivalentTo(expected!.Author);
		result.Issue.Should().BeEquivalentTo(expected!.Issue);

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

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}
}