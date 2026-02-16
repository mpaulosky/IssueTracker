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
/// <remarks>
/// Stub implementation for Phase 1. Phase 2 will add tracing, metrics, and logging exporters.
/// </remarks>
public static class OpenTelemetryExtensions
{
	/// <summary>
	/// Configures OpenTelemetry exporters for Aspire dashboard integration.
	/// </summary>
	/// <param name="builder">The host application builder.</param>
	/// <returns>The builder for chaining.</returns>
	public static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
	{
		// Stub: Phase 2 will implement OpenTelemetry configuration:
		// - AddOpenTelemetry()
		// - WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation())
		// - WithTracing(tracing => tracing.AddAspNetCoreInstrumentation())

		return builder;
	}
}
