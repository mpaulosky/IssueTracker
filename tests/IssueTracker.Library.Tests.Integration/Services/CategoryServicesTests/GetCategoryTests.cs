namespace IssueTracker.Library.Services.CategoryServicesTests;

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
		var expected = FakeCategory.GetNewCategory();
		await _sut.CreateCategory(expected);

		// Act
		var result = await _sut.GetCategory(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task GetCategory_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		_cleanupValue = "";
		var id = "62cf2ad6326e99d665759e5a";

		// Act
		var result = await _sut.GetCategory(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetCategory_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";
		string id = null;

		// Act
		var act = async () => await _sut.GetCategory(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	[Fact]
	public async Task GetCategory_With_EmptyStringId_Should_ThrowArgumentException_Test()
	{

		// Arrange
		_cleanupValue = "";
		var id = "";

		// Act
		var act = async () => await _sut.GetCategory(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentException>();

	}

	public Task InitializeAsync() => Task.CompletedTask;

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}