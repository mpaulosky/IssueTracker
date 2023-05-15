
namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class ArchiveStatusTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;
	private string? _cleanupValue;

	public ArchiveStatusTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new StatusRepository(context);

	}

	[Fact(DisplayName = "Archive Status With Valid Data (Archive)")]
	public async Task ArchiveAsync_With_ValidData_Should_ArchiveAStatus_TestAsync()
	{

		// Arrange
		_cleanupValue = "statuses";
		var expected = FakeStatus.GetNewStatus();

		await _sut.CreateAsync(expected);

		// Act
		await _sut.ArchiveAsync(expected);

		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Archived.Should().BeTrue();

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
