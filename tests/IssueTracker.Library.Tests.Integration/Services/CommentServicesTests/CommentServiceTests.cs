namespace IssueTracker.Library.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CommentServiceTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private ICommentRepository _repo;
	private IMemoryCache _memCache;
	private string _cleanupValue = "";

	public CommentServiceTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		_repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		_memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
	}

	[Fact]
	public void CommentService_With_NullCommentRepository_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_repo = null;

		// Act
		var act = () => new CommentService(_repo, _memCache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	[Fact]
	public void CommentService_With_NullMemoryCache_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_memCache = null;

		// Act
		var act = () => new CommentService(_repo, _memCache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	public Task InitializeAsync() => Task.CompletedTask;

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}