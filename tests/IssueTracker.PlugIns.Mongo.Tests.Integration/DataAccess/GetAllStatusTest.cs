namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetAllStatusTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;
	private const string? CleanupValue = "statuses";

	public GetAllStatusTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new StatusRepository(context);

	}

	[Theory(DisplayName = "GetAllAsync With Valid Data Should Succeed")]
	[InlineData(3, false, 2)]
	[InlineData(3, true, 3)]
	public async Task GetAllAsync_With_Valid_Data_Should_Succeed_TestAsync(int count, bool includeArchived, int expected)
	{

		// Arrange
		List<StatusModel> categories = FakeStatus.GetStatuses(count).ToList();

		categories.First().Archived = true;
		var catItem = 0;

		foreach (var item in categories)
		{
			item.Archived = catItem == 0;

			item.Id = string.Empty;
			catItem++;
			await _sut.CreateAsync(item).ConfigureAwait(false);

		}

		// Act
		List<StatusModel> result = (await _sut.GetAllAsync(includeArchived).ConfigureAwait(false))!.ToList();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(expected);

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
