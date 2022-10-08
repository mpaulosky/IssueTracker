namespace IssueTracker.UI.Tests.Integration.Services.CategoryServicesTests;

public class GetCategoriesTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private CategoryService _sut;

	public GetCategoriesTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = (ICategoryRepository)_factory.Services.GetRequiredService(typeof(ICategoryRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CategoryService(repo, memCache);

	}

	[Fact]
	public async Task GetCategories_With_ValidData_Should_ReturnCategories_Test()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();
		await _sut.CreateCategory(expected);

		// Act
		var result = await _sut.GetCategories();

		// Assert
		result[0].Should().BeEquivalentTo(expected);

	}

}
