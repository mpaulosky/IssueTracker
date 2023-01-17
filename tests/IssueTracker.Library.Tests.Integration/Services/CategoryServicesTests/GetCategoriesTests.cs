namespace IssueTracker.Library.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCategoriesTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryService _sut;
	private string _cleanupValue;

	public GetCategoriesTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var db = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
		db.Database.DropCollection(CollectionNames.GetCollectionName(nameof(CategoryModel)));

		var repo = (ICategoryRepository)_factory.Services.GetRequiredService(typeof(ICategoryRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CategoryService(repo, memCache);

	}

	[Fact]
	public async Task GetCategories_With_ValidData_Should_ReturnCategories_Test()
	{

		// Arrange
		_cleanupValue = "categories";
		CategoryModel expected = FakeCategory.GetNewCategory();
		await _sut.CreateCategory(expected);

		// Act
		List<CategoryModel> results = await _sut.GetCategories();

		// Assert
		results.Count.Should().Be(1);
		results.First().CategoryName.Should().Be(expected.CategoryName);
		results.First().CategoryDescription.Should().Be(expected.CategoryDescription);

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