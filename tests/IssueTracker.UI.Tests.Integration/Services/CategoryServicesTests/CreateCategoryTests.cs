namespace IssueTracker.Library.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class CreateCategoryTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryService _sut;

	public CreateCategoryTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (ICategoryRepository)_factory.Services.GetRequiredService(typeof(ICategoryRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CategoryService(repo, memCache);

	}

	[Fact]
	public async Task CreateCategory_With_ValidData_Should_CreateACategory_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();

		// Act
		await _sut.CreateCategory(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateCategory_With_InValidData_Should_FailToCreateACategory_TestAsync()
	{

		// Arrange
		CategoryModel expected = null;

		// Act
		var act = async () => await _sut.CreateCategory(expected);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

}