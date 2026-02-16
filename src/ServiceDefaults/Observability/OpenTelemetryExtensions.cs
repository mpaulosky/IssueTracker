// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     OpenTelemetryExtensions.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  ServiceDefaults
// =============================================

namespace ServiceDefaults.Observability;

/// <summary>
/// Extension methods for configuring OpenTelemetry integration with Aspire.
/// </summary>
public static class OpenTelemetryExtensions
{
	/// <summary>
	/// Configures OpenTelemetry exporters for Aspire dashboard integration.
	/// </summary>
	/// <param name="builder">The host application builder.</param>
	/// <returns>The builder for chaining.</returns>
	public static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
	{
		var isProduction = builder.Environment.IsProduction();

		builder.Services.AddOpenTelemetry()
			.WithMetrics(metrics => metrics
				.AddAspNetCoreInstrumentation()
				.AddHttpClientInstrumentation()
				.AddRuntimeInstrumentation()
				.AddConsoleExporter()
				.AddOtlpExporter())
			.WithTracing(tracing => tracing
				.AddAspNetCoreInstrumentation()
				.AddHttpClientInstrumentation()
				.SetSampler(isProduction ? new TraceIdRatioBasedSampler(0.1) : new AlwaysOnSampler())
				.AddConsoleExporter()
				.AddOtlpExporter());

		return builder;
	}
}
