namespace IssueTracker.UI.Tests.Integration.Services.CategoryServicesTests;

public class CategoryServiceTests : IClassFixture<IssueTrackerUIFactory>
{
	private readonly IssueTrackerUIFactory _factory;
	private ICategoryRepository _repo;
	private IMemoryCache _cache;

	public CategoryServiceTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		_repo = (ICategoryRepository)_factory.Services.GetRequiredService(typeof(ICategoryRepository));
		_cache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));

	}

	[Fact]
	public async Task CategoryService_With_InvalidCategoryRepository_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_repo = null;

		// Act
		var act = () => new CategoryService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	[Fact]
	public async Task CategoryService_With_InvalidMemoryCache_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cache = null;

		// Act
		var act = () => new CategoryService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}


}
