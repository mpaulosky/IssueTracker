// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Extensions.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  ServiceDefaults
// =============================================

namespace ServiceDefaults;

/// <summary>
/// Extension methods for registering shared infrastructure services.
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Registers ServiceDefaults infrastructure: health checks, observability, and problem details.
	/// </summary>
	/// <param name="builder">The host application builder.</param>
	/// <returns>The builder for chaining.</returns>
	public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
	{
	// OpenTelemetry: Tracing, Metrics, Logging
	Observability.OpenTelemetryExtensions.AddOpenTelemetryExporters(builder);

	// Distributed Cache: Redis for session and distributed caching (optional in tests)
	var redisEndpoint = builder.Configuration.GetValue<string>("Redis:Endpoint") ?? "localhost:6379";
	var enableRedis = builder.Configuration.GetValue<bool>("Redis:Enabled", true);

	if (enableRedis)
	{
		builder.Services.AddStackExchangeRedisCache(options =>
		{
			options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
			{
				EndPoints = { redisEndpoint },
				AbortOnConnectFail = false,
				ConnectTimeout = 2000,
				SyncTimeout = 2000,
			};
		});

		// Redis Connection: Required by RedisHealthCheck
		builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
		{
			var config = new StackExchange.Redis.ConfigurationOptions
			{
				EndPoints = { redisEndpoint },
				AbortOnConnectFail = false,
				ConnectTimeout = 2000,
				SyncTimeout = 2000,
			};
			return ConnectionMultiplexer.Connect(config);
		});
	}

	// Cache Service: Wrapper around IDistributedCache with JSON serialization
	builder.Services.AddScoped<ICacheService, CacheService>();

	// Health Checks: MongoDB connectivity and optional Redis connectivity
	var healthChecks = builder.Services.AddHealthChecks()
		.AddCheck<HealthChecks.MongoDbHealthCheck>("mongodb");

	if (enableRedis)
	{
		healthChecks.AddCheck<HealthChecks.RedisHealthCheck>("redis");
	}

	// Problem Details: RFC 7807 standardized error responses
	builder.Services.AddProblemDetails();

	return builder;
	}

	/// <summary>
	/// Maps default endpoints including health checks and other diagnostic endpoints.
	/// </summary>
	/// <param name="app">The web application.</param>
	/// <returns>The application for chaining.</returns>
	public static WebApplication MapDefaultEndpoints(this WebApplication app)
	{
		// Map health check endpoints
		app.MapHealthChecks("/health");

		return app;
	}
}
