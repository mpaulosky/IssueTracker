namespace IssueTracker.PlugIns.Mongo.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CategoryServiceTests : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private ICategoryRepository _repo;
	private IMemoryCache _cache;
	private const string _cleanupValue = "";

	public CategoryServiceTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		_repo = (ICategoryRepository)_factory.Services.GetRequiredService(typeof(ICategoryRepository));
		_cache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));

	}

	[Fact]
	public void CategoryService_With_InvalidCategoryRepository_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_repo = null;

		// Act
		Func<CategoryService> act = () => new CategoryService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	[Fact]
	public void CategoryService_With_InvalidMemoryCache_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cache = null;

		// Act
		Func<CategoryService> act = () => new CategoryService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}