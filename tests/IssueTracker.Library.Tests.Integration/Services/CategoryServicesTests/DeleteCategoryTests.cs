namespace IssueTracker.Library.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class DeleteCategoryTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryService _sut;
	private string _cleanupValue;

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
		_cleanupValue = "categories";
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
		_cleanupValue = "";
		CategoryModel expected = null;

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.DeleteCategory(expected));

	}

	public Task InitializeAsync() => Task.CompletedTask;

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}
