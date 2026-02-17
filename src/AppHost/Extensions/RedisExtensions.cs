// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     RedisExtensions.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  AppHost
// =============================================

using IssueTracker.AppHost.Helpers;

namespace AppHost.Extensions;

/// <summary>
/// Extension methods for configuring Redis resources in Aspire.
/// </summary>
public static class RedisExtensions
{
	/// <summary>
	/// Adds a Redis cache resource to the distributed application with standard configuration and dashboard commands.
	/// </summary>
	/// <param name="builder">The <see cref="IDistributedApplicationBuilder"/> to add the Redis resource to.</param>
	/// <param name="name">The name of the Redis resource. Defaults to "redis".</param>
	/// <param name="database">The database number to use. Defaults to 0.</param>
	/// <returns>
	/// An <see cref="IResourceBuilder{T}"/> for the added Redis resource, which can be used
	/// to further configure the resource or add references to other services.
	/// </returns>
	/// <remarks>
	/// This extension method configures Redis with:
	/// - Data volume for persistence
	/// - Health check enabled
	/// - Dashboard commands for cache management (Clear Cache, Clear All Databases)
	/// 
	/// Example usage:
	/// <code>
	/// var redis = builder.AddRedisCache("redis");
	/// var ui = builder
	///     .AddProject&lt;Projects.IssueTracker_UI&gt;("ui")
	///     .WithReference(redis);
	/// </code>
	/// </remarks>
	public static IResourceBuilder<RedisResource> AddRedisCache(
		this IDistributedApplicationBuilder builder,
		string name = "redis",
		int database = 0)
	{
		if (builder is null)
		{
			throw new ArgumentNullException(nameof(builder));
		}

		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Name cannot be null or empty.", nameof(name));
		}

		if (database < 0)
		{
			throw new ArgumentException("Database number cannot be negative.", nameof(database));
		}

		var redis = builder
			.AddRedis(name)
			.WithDataVolume()
			.WithHealthCheck(name);

		// Add dashboard command to clear the specified Redis database
		var clearCacheOptions = new CommandOptions
		{
			UpdateState = context => ResourceCommandState.Enabled,
			IconName = "Delete",
			IconVariant = IconVariant.Filled,
			IsHighlighted = false,
			Description = $"Clears all data from database {database}"
		};

		redis.WithCommand(
			name: "clear-cache",
			displayName: "Clear Cache",
			executeCommand: async context =>
			{
				var connectionString = await redis.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);
				
				if (string.IsNullOrWhiteSpace(connectionString))
				{
					return CommandResults.Failure();
				}

				try
				{
					await CacheAdminHelper.ClearRedisDatabaseAsync(connectionString, database);
					return CommandResults.Success();
				}
				catch (RedisException)
				{
					// Redis-specific errors (connection, authentication, etc.)
					return CommandResults.Failure();
				}
				catch (OperationCanceledException)
				{
					// Operation was cancelled by user or timeout
					return CommandResults.Failure();
				}
			},
			commandOptions: clearCacheOptions);

		// Add dashboard command to clear all Redis databases
		var clearAllDatabasesOptions = new CommandOptions
		{
			UpdateState = context => ResourceCommandState.Enabled,
			IconName = "DeleteForever",
			IconVariant = IconVariant.Filled,
			IsHighlighted = true,
			Description = "Clears all data from all Redis databases",
			ConfirmationMessage = "Are you sure you want to clear all Redis databases? This action cannot be undone."
		};

		redis.WithCommand(
			name: "clear-all-databases",
			displayName: "Clear All Databases",
			executeCommand: async context =>
			{
				var connectionString = await redis.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);
				
				if (string.IsNullOrWhiteSpace(connectionString))
				{
					return CommandResults.Failure();
				}

				try
				{
					await CacheAdminHelper.ClearAllRedisDatabasesAsync(connectionString);
					return CommandResults.Success();
				}
				catch (RedisException)
				{
					// Redis-specific errors (connection, authentication, etc.)
					return CommandResults.Failure();
				}
				catch (OperationCanceledException)
				{
					// Operation was cancelled by user or timeout
					return CommandResults.Failure();
				}
			},
			commandOptions: clearAllDatabasesOptions);

		return redis;
	}
}
