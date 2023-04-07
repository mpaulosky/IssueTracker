using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
using IssueTracker.PlugIns.Services;

namespace IssueTracker.PlugIns.Mongo.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCategoryTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryService _sut;
	private string _cleanupValue;

	public GetCategoryTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (ICategoryRepository)_factory.Services.GetRequiredService(typeof(ICategoryRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CategoryService(repo, memCache);

	}

	[Fact]
	public async Task GetCategory_With_WithData_Should_ReturnAValidCategory_TestAsync()
	{

		// Arrange
		_cleanupValue = "categories";
		CategoryModel expected = FakeCategory.GetNewCategory();
		await _sut.CreateCategory(expected);

		// Act
		CategoryModel result = await _sut.GetCategory(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task GetCategory_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		_cleanupValue = "";
		const string id = "62cf2ad6326e99d665759e5a";

		// Act
		CategoryModel result = await _sut.GetCategory(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetCategory_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";

		// Act
		Func<Task<CategoryModel>> act = async () => await _sut.GetCategory(null);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	[Fact]
	public async Task GetCategory_With_EmptyStringId_Should_ThrowArgumentException_Test()
	{

		// Arrange
		_cleanupValue = "";
		const string id = "";

		// Act
		Func<Task<CategoryModel>> act = async () => await _sut.GetCategory(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentException>();

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