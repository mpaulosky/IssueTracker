namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateStatusTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;
	private const string CleanupValue = "statuses";

	public CreateStatusTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new StatusRepository(context);

	}

	[Fact]
	public async Task CreateAsync_With_ValidData_Should_CreateAStatus_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus();

		// Act
		await _sut.CreateAsync(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateAsync_With_InValidData_Should_FailToCreateAStatus_TestAsync()
	{

		// Arrange

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateAsync(null!));

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
