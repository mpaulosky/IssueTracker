// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     MongoHealthCheckTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI.Tests.Unit
// =============================================

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IssueTracker.UI.Helpers;

[ExcludeFromCodeCoverage]
public class MongoHealthCheckTests : TestContext
{
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private readonly Mock<IMongoDbContextFactory> _mockContextWithoutDatabase;

	public MongoHealthCheckTests()
	{
		_mockContext = TestFixtures.GetMockContext();

		_mockContextWithoutDatabase = TestFixtures.GetMockContextWithOutDataBase();
	}

	private MongoHealthCheck CreateMongoHealthCheck(bool withDatabase = true)
	{
		return withDatabase switch
		{
			true => new MongoHealthCheck(_mockContext.Object),
			_ => new MongoHealthCheck(_mockContextWithoutDatabase.Object)
		};
	}

	[Fact]
	public async Task CheckHealthAsync_With_Mock_Database_Returns_Healthy_Status_TestAsync()
	{
		// Arrange
		MongoHealthCheck mongoHealthCheck = CreateMongoHealthCheck();
		HealthCheckContext context = new();
		CancellationToken cancellationToken = new();

		// Act
		HealthCheckResult result = await mongoHealthCheck.CheckHealthAsync(
			context,
			cancellationToken).ConfigureAwait(false);

		// Assert
		result.Status.Should().Be(HealthStatus.Healthy);
	}

	[Fact]
	public async Task CheckHealthAsync_WithOut_Mock_Database_Returns_UnHealthy_Status_TestAsync()
	{
		// Arrange
		MongoHealthCheck mongoHealthCheck = CreateMongoHealthCheck(false);
		HealthCheckContext context = new();
		CancellationToken cancellationToken = new();

		// Act
		HealthCheckResult result = await mongoHealthCheck.CheckHealthAsync(
			context,
			cancellationToken).ConfigureAwait(false);

		// Assert
		result.Status.Should().Be(HealthStatus.Unhealthy);
	}
}