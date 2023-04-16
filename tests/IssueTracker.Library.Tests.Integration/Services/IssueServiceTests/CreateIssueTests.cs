namespace IssueTracker.PlugIns.Services.IssueServiceTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateIssueTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;
	private string _cleanupValue;

	public CreateIssueTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task CreateIssue_With_ValidData_Should_CreateAIssue_TestAsync()
	{

		// Arrange
		_cleanupValue = "issues";
		var expected = FakeIssue.GetNewIssue();

		// Act
		await _sut.CreateIssue(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateIssue_With_InValidData_Should_FailToCreateAIssue_TestAsync()
	{

		// Arrange
		_cleanupValue = "";

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateIssue(null));

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
