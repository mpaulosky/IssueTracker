namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateIssueTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueRepository _sut;
	private const string CleanupValue = "issues";

	public CreateIssueTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new IssueRepository(context);

	}

	[Fact]
	public async Task CreateAsync_With_ValidData_Should_CreateAIssue_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();

		// Act
		await _sut.CreateAsync(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateAsync_With_InValidData_Should_FailToCreateAIssue_TestAsync()
	{

		// Arrange

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateAsync(null!));

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
