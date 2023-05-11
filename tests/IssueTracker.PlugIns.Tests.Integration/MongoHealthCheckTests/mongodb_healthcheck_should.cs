namespace IssueTracker.PlugIns.Tests.Integration.MongoHealthCheckTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class MongodbHealthcheckShould : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private TestServer? _sut;
	private const string? CleanupValue = "";

	public MongodbHealthcheckShould(IssueTrackerTestFactory factory)
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

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
