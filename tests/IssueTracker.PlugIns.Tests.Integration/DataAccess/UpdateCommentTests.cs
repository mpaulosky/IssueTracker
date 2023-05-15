namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateCommentTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;
	private string? _cleanupValue;

	public UpdateCommentTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CommentRepository(context);

	}

	[Fact(DisplayName = "UpdateAsync With Valid Data Should Succeed")]
	public async Task UpdateAsync_With_ValidData_Should_UpdateTheComment_Test()
	{

		// Arrange
		_cleanupValue = "comments";
		var expected = FakeComment.GetNewComment();
		await _sut.CreateAsync(expected);

		// Act
		expected.Title = "Updated";
		await _sut.UpdateAsync(expected.Id, expected);
		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Author.Should().BeEquivalentTo(expected.Author);
		result.CommentOnSource!.SourceType.Should().Be(expected.CommentOnSource!.SourceType);

	}

	[Fact(DisplayName = "UpdateAsync With Invalid Data Should Fail")]
	public async Task UpdateAsync_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";

		// Act
		Func<Task> act = async () => await _sut.UpdateAsync(null!, null!);

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
