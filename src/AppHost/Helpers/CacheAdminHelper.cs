// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CacheAdminHelper.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  AppHost
// =============================================

namespace IssueTracker.AppHost.Helpers;

/// <summary>
/// Helper class for cache administration tasks such as clearing Redis cache.
/// </summary>
public static class CacheAdminHelper
{
	/// <summary>
	/// Clears all data from the specified Redis database.
	/// </summary>
	/// <param name="connectionString">The Redis connection string.</param>
	/// <param name="database">The database number to clear. Defaults to 0.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <remarks>
	/// This method connects to Redis, flushes the specified database,
	/// and handles any connection errors gracefully.
	/// 
	/// Example usage:
	/// <code>
	/// var connectionString = "localhost:6379";
	/// await CacheAdminHelper.ClearRedisDatabaseAsync(connectionString, 0);
	/// </code>
	/// </remarks>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionString"/> is null or empty.</exception>
	public static async Task ClearRedisDatabaseAsync(
		string connectionString,
		int database = 0)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
		}

		if (database < 0)
		{
			throw new ArgumentException("Database number cannot be negative.", nameof(database));
		}

		try
		{
			using var connection = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync(connectionString);
			var db = connection.GetDatabase(database);
			await db.ExecuteAsync("FLUSHDB");
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException(
				$"Failed to clear Redis database {database}. See inner exception for details.",
				ex);
		}
	}

	/// <summary>
	/// Clears all data from all Redis databases.
	/// </summary>
	/// <param name="connectionString">The Redis connection string.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <remarks>
	/// This method connects to Redis and flushes all databases using the FLUSHALL command.
	/// Use with caution as this clears all data across all databases.
	/// 
	/// Example usage:
	/// <code>
	/// var connectionString = "localhost:6379";
	/// await CacheAdminHelper.ClearAllRedisDatabasesAsync(connectionString);
	/// </code>
	/// </remarks>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionString"/> is null or empty.</exception>
	public static async Task ClearAllRedisDatabasesAsync(string connectionString)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
		}

		try
		{
			using var connection = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync(connectionString);
			await connection.GetServer(connection.GetEndPoints().FirstOrDefault()
				?? throw new InvalidOperationException("No Redis endpoints found."))
				.ExecuteAsync("FLUSHALL");
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException(
				"Failed to clear all Redis databases. See inner exception for details.",
				ex);
		}
	}

	/// <summary>
	/// Gets information about the Redis server including memory usage and connected clients.
	/// </summary>
	/// <param name="connectionString">The Redis connection string.</param>
	/// <returns>A dictionary containing Redis server information.</returns>
	/// <remarks>
	/// This method is useful for monitoring cache health and usage.
	/// </remarks>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionString"/> is null or empty.</exception>
	public static async Task<Dictionary<string, string>> GetRedisInfoAsync(string connectionString)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
		}

		var info = new Dictionary<string, string>();

		try
		{
			using var connection = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync(connectionString);
			var server = connection.GetServer(connection.GetEndPoints().FirstOrDefault()
				?? throw new InvalidOperationException("No Redis endpoints found."));
			
			var serverInfo = server.Info();
			
			foreach (var group in serverInfo)
			{
				foreach (var item in group)
				{
					info[$"{group.Key}:{item.Key}"] = item.Value;
				}
			}
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException(
				"Failed to retrieve Redis information. See inner exception for details.",
				ex);
		}

		return info;
	}
}
