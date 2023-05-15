namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateCategoryTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;
	private const string? CleanupValue = "categories";

	public CreateCategoryTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CategoryRepository(context);

	}

	[Fact(DisplayName = "CreateAsync With Valid Data Should Succeed")]
	public async Task CreateAsync_With_Valid_Data_Should_Succeed_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();

		// Act
		await _sut.CreateAsync(expected).ConfigureAwait(false);
		CategoryModel? result = (await _sut.GetAsync(expected.Id).ConfigureAwait(false));

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.CategoryName.Should().Be(expected.CategoryName);
		result.CategoryDescription.Should().Be(expected.CategoryDescription);
		result.Archived.Should().Be(expected.Archived);

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
