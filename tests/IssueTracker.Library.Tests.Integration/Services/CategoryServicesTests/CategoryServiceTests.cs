using IssueTracker.Library;

namespace IssueTracker.Library.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class CategoryServiceTests : IClassFixture<IssueTrackerTestFactory>
{
	private readonly IssueTrackerTestFactory _factory;
	private ICategoryRepository _repo;
	private IMemoryCache _cache;

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
		var act = () => new CategoryService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	[Fact]
	public void CategoryService_With_InvalidMemoryCache_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cache = null;

		// Act
		var act = () => new CategoryService(_repo, _cache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

}
