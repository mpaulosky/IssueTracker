namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateCategoryTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;
	private const string CleanupValue = "categories";
	public UpdateCategoryTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CategoryRepository(context);

	}

	[Fact(DisplayName = "UpdateAsync With Valid Data Should Succeed")]
	public async Task UpdateAsync_With_ValidData_Should_UpdateTheCategory_Test()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory(true);
		await _sut.CreateAsync(expected);

		// Act
		expected.CategoryDescription = "Updated";
		await _sut.UpdateAsync(expected.Id, expected);

		var result = (await _sut.GetAllAsync()).First();

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact(DisplayName = "UpdateAsync With In Valid Data Should Fail")]
	public async Task UpdateAsync_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange

		// Act
		Func<Task> act = async () => await _sut.UpdateAsync(null!, null!);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
