namespace IssueTracker.PlugIns.Tests.Integration.Services.StatusServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class StatusServiceTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private IStatusRepository _repo;
	private IMemoryCache _cache;
	private const string? CleanupValue = "";

	public StatusServiceTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		_repo = (IStatusRepository)_factory.Services.GetRequiredService(typeof(IStatusRepository));
		_cache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));

	}

	[Fact]
	public void StatusService_With_InvalidStatusRepository_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_repo = null!;

		// Act
		Func<StatusService> act = () => new StatusService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	[Fact]
	public void StatusService_With_InvalidMemoryCache_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cache = null!;

		// Act
		Func<StatusService> act = () => new StatusService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

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
