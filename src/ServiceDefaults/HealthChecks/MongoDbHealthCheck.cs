// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     MongoDbHealthCheck.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  ServiceDefaults
// =============================================

namespace ServiceDefaults.HealthChecks;

/// <summary>
/// Health check for MongoDB connectivity.
/// </summary>
/// <remarks>
/// Stub implementation for Phase 1. Phase 2 will add actual MongoDB ping logic.
/// </remarks>
public sealed class MongoDbHealthCheck : IHealthCheck
{
	/// <summary>
	/// Checks MongoDB connectivity by pinging the database.
	/// </summary>
	/// <param name="context">The health check context.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>A task representing the health check result.</returns>
	public Task<HealthCheckResult> CheckHealthAsync(
		HealthCheckContext context,
		CancellationToken cancellationToken = default)
	{
		// Stub: Phase 2 will implement actual MongoDB ping using IMongoClient
		return Task.FromResult(HealthCheckResult.Healthy("MongoDB health check not yet implemented"));
	}
}
