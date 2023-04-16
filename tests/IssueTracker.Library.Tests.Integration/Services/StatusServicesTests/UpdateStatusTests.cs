namespace IssueTracker.PlugIns.Services.StatusServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateStatusTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusService _sut;
	private string _cleanupValue;

	public UpdateStatusTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IStatusRepository)_factory.Services.GetRequiredService(typeof(IStatusRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new StatusService(repo, memCache);

	}

	[Fact]
	public async Task UpdateStatus_With_ValidData_Should_UpdateTheStatus_Test()
	{

		// Arrange
		_cleanupValue = "statuses";
		var expected = FakeStatus.GetNewStatus();
		await _sut.CreateStatus(expected);

		// Act
		expected.StatusDescription = "Updated";
		await _sut.UpdateStatus(expected);
		var result = await _sut.GetStatus(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task UpdateStatus_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";

		// Act
		Func<Task> act = async () => await _sut.UpdateStatus(null);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

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
