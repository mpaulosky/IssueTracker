namespace IssueTracker.Library.MongoHealthCheckTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class Mongodb_healthcheck_should : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private TestServer _sut;
	private string _cleanupValue = "";

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

	public Task InitializeAsync() => Task.CompletedTask;

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}