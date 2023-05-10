
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IssueTracker.PlugIns.Tests.Unit.Helpers;

public class MongoDbHealthCheckTests
{

	private readonly Mock<IMongoDbContextFactory> _mockContext;

	public MongoDbHealthCheckTests()
	{

		_mockContext = GetMockMongoContext();

	}

	private MongoDbHealthCheck CreateMongoDbHealthCheck()
	{
		return new MongoDbHealthCheck(_mockContext.Object);
	}

	[Fact]
	public async Task CheckHealthAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var mongoDbHealthCheck = CreateMongoDbHealthCheck();

		HealthCheckContext? context = null;

		var cancellationToken = default(CancellationToken);

		// Act
		var result = await mongoDbHealthCheck.CheckHealthAsync(context!, cancellationToken);

		// Assert
		result.Should().NotBeNull();

	}
}
