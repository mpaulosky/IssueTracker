namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateCategoryTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;
	private const string CleanupValue = "categories";

	public UpdateCategoryTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CategoryRepository(context);

	}

	[Fact(DisplayName = "UpdateAsync With Valid Data Should Update Successfully")]
	public async Task UpdateAsync_With_Valid_Data_Should_Update_Successfully_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();

		await _sut.CreateAsync(expected).ConfigureAwait(false);

		var update = new CategoryModel
		{
			Id = expected.Id,
			CategoryName = expected.CategoryName,
			CategoryDescription = "Updated",
			Archived = expected.Archived
		};

		// Act
		await _sut.UpdateAsync(update).ConfigureAwait(false);

		var result = await _sut.GetAsync(expected.Id)!.ConfigureAwait(false);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.CategoryDescription.Should().Be(update.CategoryDescription);

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
