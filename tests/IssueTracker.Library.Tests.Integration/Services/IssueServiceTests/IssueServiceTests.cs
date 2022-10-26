namespace IssueTracker.Library.Services.IssueServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class IssueServiceTests : IClassFixture<IssueTrackerTestFactory>
{
	private readonly IssueTrackerTestFactory _factory;
	private IIssueRepository _repo;
	private IMemoryCache _cache;

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

}
