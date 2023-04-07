using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
using IssueTracker.PlugIns.Services;

namespace IssueTracker.PlugIns.Mongo.Services.StatusServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetStatusTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusService _sut;
	private string _cleanupValue;

	public GetStatusTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IStatusRepository)_factory.Services.GetRequiredService(typeof(IStatusRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new StatusService(repo, memCache);

	}

	[Fact]
	public async Task GetStatus_With_WithData_Should_ReturnAValidStatus_TestAsync()
	{

		// Arrange
		_cleanupValue = "statuses";
		StatusModel expected = FakeStatus.GetNewStatus();
		await _sut.CreateStatus(expected);

		// Act
		StatusModel result = await _sut.GetStatus(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task GetStatus_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		_cleanupValue = "";
		const string id = "62cf2ad6326e99d665759e5a";

		// Act
		StatusModel result = await _sut.GetStatus(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetStatus_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";

		// Act
		Func<Task<StatusModel>> act = async () => await _sut.GetStatus(null);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	[Fact]
	public async Task GetStatus_With_EmptyStringId_Should_ThrowArgumentException_Test()
	{

		// Arrange
		_cleanupValue = "";
		const string id = "";

		// Act
		Func<Task<StatusModel>> act = async () => await _sut.GetStatus(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentException>();

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
