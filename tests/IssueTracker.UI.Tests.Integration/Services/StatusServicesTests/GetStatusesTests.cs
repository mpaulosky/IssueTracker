
namespace IssueTracker.UI.Tests.Integration.Services.StatusServicesTests;

public class GetStatusesTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly StatusService _sut;

	public GetStatusesTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = (IStatusRepository)_factory.Services.GetRequiredService(typeof(IStatusRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new StatusService(repo, memCache);

	}

	[Fact]
	public async Task GetStatuses_With_ValidData_Should_ReturnStatuses_Test()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus();
		await _sut.CreateStatus(expected);

		// Act
		var result = await _sut.GetStatuses();

		// Assert
		result[0].Should().BeEquivalentTo(expected);

	}

}
