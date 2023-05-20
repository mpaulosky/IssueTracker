namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentsByUserTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;
	private const string CleanupValue = "comments";

	public GetCommentsByUserTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CommentRepository(context);

	}

	[Fact(DisplayName = "GetByUserAsync With Valid Data Should Succeed")]
	public async Task GetByUserAsync_With_ValidData_Should_ReturnValidComment_Test()
	{
		// Arrange
		var expected = FakeComment.GetNewComment();
		await _sut.CreateAsync(expected);

		// Act
		var result = (await _sut.GetByUserAsync(expected.Author.Id)).ToList();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result[0].Author.Id.Should().Be(expected.Author.Id);

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
