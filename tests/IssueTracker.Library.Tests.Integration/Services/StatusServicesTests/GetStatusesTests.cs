using Microsoft.AspNetCore.Http;

namespace IssueTracker.Library.Services.StatusServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetStatusesTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusService _sut;

	public GetStatusesTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;

		var db = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
		db.Database.DropCollection(CollectionNames.GetCollectionName(nameof(StatusModel)));

		var repo = (IStatusRepository)_factory.Services.GetRequiredService(typeof(IStatusRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		memCache.Remove("StatusData");

		_sut = new StatusService(repo, memCache);

	}

	[Fact]
	public async Task GetStatuses_With_ValidData_Should_ReturnStatuses_Test()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus();
		await _sut.CreateStatus(expected);

		// Act
		var results = await _sut.GetStatuses();

		// Assert
		results.Count.Should().Be(1);
		results.First().StatusName.Should().Be(expected.StatusName);
		results.First().StatusDescription.Should().Be(expected.StatusDescription);

	}

}
