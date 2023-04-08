namespace IssueTracker.PlugIns.Mongo.Services.CategoryServicesTests;

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
		CategoryModel expected = FakeCategory.GetNewCategory();
		await _sut.CreateCategory(expected);

		// Act
		await _sut.DeleteCategory(expected);
		CategoryModel result = await _sut.GetCategory(expected.Id);

		// Assert
		result.Archived.Should().BeTrue();

	}

	[Fact]
	public async Task DeleteCategory_With_InValidData_Should_FailToDeleteACategory_TestAsync()
	{

		// Arrange
		_cleanupValue = "";

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.DeleteCategory(null));

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
