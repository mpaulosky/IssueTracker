namespace IssueTracker.Library.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class DeleteCategoryTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryService _sut;

	public DeleteCategoryTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (ICategoryRepository)_factory.Services.GetRequiredService(typeof(ICategoryRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CategoryService(repo, memCache);

	}

	[Fact]
	public async Task DeleteCategory_With_ValidData_Should_DeleteACategory_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();
		await _sut.CreateCategory(expected);

		// Act
		await _sut.DeleteCategory(expected);
		var result = await _sut.GetCategory(expected.Id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task DeleteCategory_With_InValidData_Should_FailToDeleteACategory_TestAsync()
	{

		// Arrange
		CategoryModel expected = null;

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.DeleteCategory(expected));

	}

}
