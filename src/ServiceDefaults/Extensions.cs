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

	// Distributed Cache: Redis for session and distributed caching
	builder.Services.AddStackExchangeRedisCache(options =>
	{
		options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
		{
			EndPoints = { "localhost:6379" },
			AbortOnConnectFail = false,
			ConnectTimeout = 2000,
			SyncTimeout = 2000,
		};
	});

	// Health Checks: MongoDB connectivity, Redis connectivity, and base health endpoints
	builder.Services.AddHealthChecks()
		.AddCheck<HealthChecks.MongoDbHealthCheck>("mongodb")
		.AddCheck<HealthChecks.RedisHealthCheck>("redis");

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
