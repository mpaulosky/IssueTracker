namespace IssueTracker.Library.Services.StatusServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class DeleteStatusTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusService _sut;

	public DeleteStatusTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IStatusRepository)_factory.Services.GetRequiredService(typeof(IStatusRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new StatusService(repo, memCache);

	}

	[Fact]
	public async Task DeleteStatus_With_ValidData_Should_DeleteAStatus_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus();
		await _sut.CreateStatus(expected);

		// Act
		await _sut.DeleteStatus(expected);
		var result = await _sut.GetStatus(expected.Id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task DeleteStatus_With_InValidData_Should_FailToDeleteAStatus_TestAsync()
	{

		// Arrange
		StatusModel expected = null;

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.DeleteStatus(expected));

	}

}
