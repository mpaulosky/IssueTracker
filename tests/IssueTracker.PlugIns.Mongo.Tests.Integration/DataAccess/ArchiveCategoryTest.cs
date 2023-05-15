namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class ArchiveCategoryTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;
	private const string? CleanupValue = "categories";

	public ArchiveCategoryTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CategoryRepository(context);

	}

	[Fact(DisplayName = "ArchiveAsync With Valid Data Should Archive Successfully")]
	public async Task ArchiveAsync_With_Valid_Data_Should_Succeed_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();

		await _sut.CreateAsync(expected).ConfigureAwait(false);

		var update = new CategoryModel
		{
			Id = expected.Id,
			CategoryName = expected.CategoryName,
			CategoryDescription = expected.CategoryDescription,
			Archived = expected.Archived
		};

		// Act
		await _sut.ArchiveAsync(update).ConfigureAwait(false);

		var result = await _sut.GetAsync(expected.Id)!.ConfigureAwait(false);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.Archived.Should().BeTrue();

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
