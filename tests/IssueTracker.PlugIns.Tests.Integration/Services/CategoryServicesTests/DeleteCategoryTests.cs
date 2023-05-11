namespace IssueTracker.PlugIns.Tests.Integration.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class DeleteCategoryTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryService _sut;
	private string? _cleanupValue;

	public DeleteCategoryTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (ICategoryRepository)_factory.Services.GetRequiredService(typeof(ICategoryRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CategoryService(repo, memCache);

	}

	[Fact(DisplayName = "Delete Category With Valid Data (Archive)")]
	public async Task DeleteCategory_With_ValidData_Should_ArchiveACategory_TestAsync()
	{

		// Arrange
		_cleanupValue = "categories";
		var expected = FakeCategory.GetNewCategory();
		await _sut.CreateCategory(expected);

		// Act
		await _sut.DeleteCategory(expected);
		var result = await _sut.GetCategory(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Archived.Should().BeTrue();

	}

	[Fact(DisplayName = "Delete Category With Invalid Data Throws Error")]
	public async Task DeleteCategory_With_InValidData_Should_FailToDeleteACategory_TestAsync()
	{

		// Arrange
		_cleanupValue = "";

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.DeleteCategory(null!));

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
