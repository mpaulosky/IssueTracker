using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IssueTracker.PlugIns.Mongo.Tests.Unit.Helpers;

[ExcludeFromCodeCoverage]

public class MongoDbHealthCheckTests
{
	private readonly IMongoDbContextFactory _factory;

	public MongoDbHealthCheckTests()
	{

		DatabaseSettings settings = new DatabaseSettings("mongodb://test123", "TestDb");

		_factory = Substitute.For<MongoDbContextFactory>(settings);

	}

	private MongoDbHealthCheck CreateMongoDbHealthCheck()
	{
		return new MongoDbHealthCheck(_factory);
	}

	[Fact(Skip = "Mongo Health Checks not enabled at this time.")]
	public async Task CheckHealthAsync_StateUnderTest_ExpectedBehavior()
	{

		// Arrange
		HealthCheckResult expected = new HealthCheckResult(HealthStatus.Healthy);

		var _healthCheck = CreateMongoDbHealthCheck();

		HealthCheckContext? context = null;
		CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

		// Act
		var result = await _healthCheck.CheckHealthAsync(context, cancellationToken);

		// Assert
		result.Should().Be(expected);

	}
}
