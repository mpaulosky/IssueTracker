namespace IssueTracker.Library.MongoHealthCheckTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class Mongodb_healthcheck_should : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private TestServer _sut;

	public Mongodb_healthcheck_should(IssueTrackerTestFactory factory)
	{

		_factory = factory;

	}

	[Fact]
	public async Task Be_healthy_if_mongodb_is_available()
	{

		// Arrange
		_sut = _factory.Server;

		// Act
		var response = await _sut.CreateRequest("/health").GetAsync();


		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);

	}

}