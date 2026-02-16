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
public sealed class MongoDbHealthCheck : IHealthCheck
{
	private readonly IMongoClient _client;
	private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(3);

	/// <summary>
	/// Initializes a new instance of the <see cref="MongoDbHealthCheck"/> class.
	/// </summary>
	/// <param name="client">The MongoDB client instance.</param>
	public MongoDbHealthCheck(IMongoClient client)
	{
		_client = client;
	}

	/// <summary>
	/// Checks MongoDB connectivity by pinging the admin database.
	/// </summary>
	/// <param name="context">The health check context.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>A task representing the health check result.</returns>
	public async Task<HealthCheckResult> CheckHealthAsync(
		HealthCheckContext context,
		CancellationToken cancellationToken = default)
	{
		try
		{
			using var timeoutCts = new CancellationTokenSource(Timeout);
			using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

			var database = _client.GetDatabase("admin");
			var pingCommand = new BsonDocument("ping", 1);
			
			await database.RunCommandAsync<BsonDocument>(pingCommand, cancellationToken: linkedCts.Token);
			
			return HealthCheckResult.Healthy("MongoDB connection is responsive");
		}
		catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
		{
			return HealthCheckResult.Unhealthy("MongoDB health check was cancelled");
		}
		catch (OperationCanceledException)
		{
			return HealthCheckResult.Unhealthy($"MongoDB connection timed out after {Timeout.TotalSeconds}s");
		}
		catch (MongoException ex)
		{
			return HealthCheckResult.Unhealthy("MongoDB connection failed", ex);
		}
		catch (Exception ex)
		{
			return HealthCheckResult.Unhealthy("Unexpected error checking MongoDB health", ex);
		}
	}
}
