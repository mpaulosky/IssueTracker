namespace IssueTracker.Library.Services.StatusServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class CreateStatusTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusService _sut;

	public CreateStatusTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IStatusRepository)_factory.Services.GetRequiredService(typeof(IStatusRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new StatusService(repo, memCache);

	}

	[Fact]
	public async Task CreateStatus_With_ValidData_Should_CreateAStatus_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus();

		// Act
		await _sut.CreateStatus(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateStatus_With_InValidData_Should_FailToCreateAStatus_TestAsync()
	{

		// Arrange
		StatusModel expected = null;

		// Act
		var act = async () => await _sut.CreateStatus(status: expected!);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

}