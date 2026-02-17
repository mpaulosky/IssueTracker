// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     RedisHealthCheck.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  ServiceDefaults
// =============================================

namespace ServiceDefaults.HealthChecks;

/// <summary>
/// Health check for Redis connectivity.
/// </summary>
public sealed class RedisHealthCheck : IHealthCheck
{
	private readonly IConnectionMultiplexer _connection;
	private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(2);

	/// <summary>
	/// Initializes a new instance of the <see cref="RedisHealthCheck"/> class.
	/// </summary>
	/// <param name="connection">The Redis connection multiplexer instance.</param>
	public RedisHealthCheck(IConnectionMultiplexer connection)
	{
		_connection = connection;
	}

	/// <summary>
	/// Checks Redis connectivity by pinging the server.
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

			var server = _connection.GetServer(_connection.GetEndPoints().First());
			var pong = await server.PingAsync(flags: CommandFlags.DemandMaster);

			if (pong != TimeSpan.Zero)
			{
				return HealthCheckResult.Healthy("Redis connection is responsive");
			}

			return HealthCheckResult.Unhealthy("Redis ping returned zero response time");
		}
		catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
		{
			return HealthCheckResult.Unhealthy("Redis health check was cancelled");
		}
		catch (OperationCanceledException)
		{
			return HealthCheckResult.Unhealthy($"Redis connection timed out after {Timeout.TotalSeconds}s");
		}
		catch (Exception ex)
		{
			return HealthCheckResult.Unhealthy("Redis connection failed", ex);
		}
	}
}
