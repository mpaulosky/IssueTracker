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
		// Placeholder for Phase 2 implementation:
		// - OpenTelemetry (tracing, metrics, logging)
		// - Health checks (MongoDB connectivity)
		// - Problem details (RFC 7807)
		// - Service discovery

		return builder;
	}
}
