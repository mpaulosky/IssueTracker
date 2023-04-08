namespace IssueTracker.PlugIns.Mongo.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateCommentTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;
	private string _cleanupValue;

	public CreateCommentTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task CreateComment_With_ValidData_Should_CreateAComment_TestAsync()
	{

		// Arrange
		_cleanupValue = "comments";
		CommentModel expected = FakeComment.GetNewComment();

		// Act
		await _sut.CreateComment(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateComment_With_InValidData_Should_FailToCreateAComment_TestAsync()
	{

		// Arrange
		_cleanupValue = "";

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateComment(null));

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
