namespace IssueTracker.Library.Services.IssueServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class IssueServiceTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private IIssueRepository _repo;
	private IMemoryCache _cache;
	private string _cleanupValue = "";

	public IssueServiceTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		_repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		_cache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));

	}

	[Fact]
	public void IssueService_With_InvalidIssueRepository_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_repo = null;

		// Act
		var act = () => new IssueService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	[Fact]
	public void IssueService_With_InvalidMemoryCache_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cache = null;

		// Act
		var act = () => new IssueService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	public Task InitializeAsync() => Task.CompletedTask;

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}
