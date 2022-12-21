namespace IssueTracker.Library.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateCategoryTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryService _sut;
	private string _cleanupValue;

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
		_cleanupValue = "categories";
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
		_cleanupValue = "";
		CategoryModel expected = null;

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateCategory(expected));

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