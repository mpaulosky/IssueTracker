namespace IssueTracker.UI.Tests.Integration.MongoHealthCheckTests;

[ExcludeFromCodeCoverage]
public class Mongodb_healthcheck_should : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private TestServer? _sut;

	public Mongodb_healthcheck_should(IssueTrackerUIFactory factory)
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