
namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class ArchiveCommentTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;
	private const string CleanupValue = "comments";

	public ArchiveCommentTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CommentRepository(context);

	}

	[Fact(DisplayName = "Archive Comment With Valid Data (Archive)")]
	public async Task ArchiveAsync_With_ValidData_Should_ArchiveAComment_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetNewComment();

		await _sut.CreateAsync(expected);

		// Act
		await _sut.ArchiveAsync(expected);

		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Archived.Should().BeTrue();

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
