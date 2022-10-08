namespace IssueTracker.UI.Tests.Integration.Services.CategoryServicesTests;

public class UpdateCategoryTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly CategoryService _sut;

	public UpdateCategoryTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = (ICategoryRepository)_factory.Services.GetRequiredService(typeof(ICategoryRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CategoryService(repo, memCache);

	}

	[Fact]
	public async Task UpdateCategory_With_ValidData_Should_UpdateTheCategory_Test()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();
		await _sut.CreateCategory(expected);

		// Act
		expected.CategoryDescription = "Updated";
		await _sut.UpdateCategory(expected);
		var result = await _sut.GetCategory(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task UpdateCategory_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		CategoryModel? expected = null;

		// Act
		var act = async () => await _sut.UpdateCategory(expected);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

}
