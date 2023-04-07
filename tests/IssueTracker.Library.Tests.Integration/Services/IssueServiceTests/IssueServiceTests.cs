using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
using IssueTracker.PlugIns.Services;

namespace IssueTracker.PlugIns.Mongo.Services.IssueServiceTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class IssueServiceTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private IIssueRepository _repo;
	private IMemoryCache _cache;
	private const string _cleanupValue = "";

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
		Func<IssueService> act = () => new IssueService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	[Fact]
	public void IssueService_With_InvalidMemoryCache_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cache = null;

		// Act
		Func<IssueService> act = () => new IssueService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

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