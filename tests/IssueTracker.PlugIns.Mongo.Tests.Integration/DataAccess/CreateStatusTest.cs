namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateStatusTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;
	private const string CleanupValue = "statuses";

	public CreateStatusTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new StatusRepository(context);

	}

	[Fact(DisplayName = "CreateAsync With Valid Data Should Succeed")]
	public async Task CreateAsync_With_Valid_Data_Should_Succeed_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus();

		// Act
		await _sut.CreateAsync(expected).ConfigureAwait(false);
		StatusModel? result = await _sut.GetAsync(expected.Id).ConfigureAwait(false);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.StatusName.Should().Be(expected.StatusName);
		result.StatusDescription.Should().Be(expected.StatusDescription);
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
