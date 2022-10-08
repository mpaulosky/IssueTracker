namespace IssueTracker.UI.Tests.Integration.Services.StatusServicesTests;

public class UpdateStatusTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly StatusService _sut;

	public UpdateStatusTests(IssueTrackerUIFactory factory)
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
		StatusModel? expected = null;

		// Act
		var act = async () => await _sut.UpdateStatus(expected);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

}
