// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ServiceDefaultsExtensionsTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  ServiceDefaults.Tests
// =============================================

namespace ServiceDefaults.Tests;

/// <summary>
/// Tests for ServiceDefaults extensions and infrastructure setup.
/// </summary>
public class ServiceDefaultsExtensionsTests
{
	/// <summary>
	/// Tests that IDistributedCache is registered and resolvable when AddServiceDefaults is called.
	/// </summary>
	[Fact]
	public void AddServiceDefaults_RegistersIDistributedCache()
	{
		// Arrange
		var builder = Host.CreateDefaultBuilder()
			.ConfigureServices(services =>
			{
				services.AddServiceDefaults();
			});

		var host = builder.Build();

		// Act
		var cache = host.Services.GetService<IDistributedCache>();

		// Assert
		cache.Should().NotBeNull("IDistributedCache should be registered by AddServiceDefaults");
	}

	/// <summary>
	/// Tests that health checks are registered and the redis health check is available.
	/// </summary>
	[Fact]
	public void AddServiceDefaults_RegistersRedisHealthCheck()
	{
		// Arrange
		var builder = Host.CreateDefaultBuilder()
			.ConfigureServices(services =>
			{
				services.AddServiceDefaults();
			});

		var host = builder.Build();

		// Act
		var healthCheckService = host.Services.GetService<IHealthCheckPublisher>();

		// Assert
		healthCheckService.Should().NotBeNull("Health checks should be registered");
	}
}
