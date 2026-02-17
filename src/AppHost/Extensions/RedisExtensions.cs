// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     RedisExtensions.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  AppHost
// =============================================

namespace IssueTracker.AppHost.Extensions;

/// <summary>
/// Extension methods for configuring Redis resources in Aspire.
/// </summary>
public static class RedisExtensions
{
	/// <summary>
	/// Adds a Redis cache resource to the distributed application with standard configuration.
	/// </summary>
	/// <param name="builder">The <see cref="IDistributedApplicationBuilder"/> to add the Redis resource to.</param>
	/// <param name="name">The name of the Redis resource. Defaults to "redis".</param>
	/// <returns>
	/// An <see cref="IResourceBuilder{T}"/> for the added Redis resource, which can be used
	/// to further configure the resource or add references to other services.
	/// </returns>
	/// <remarks>
	/// This extension method configures Redis with:
	/// - Data volume for persistence
	/// - Health check enabled
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
		string name = "redis")
	{
		if (builder is null)
		{
			throw new ArgumentNullException(nameof(builder));
		}

		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Name cannot be null or empty.", nameof(name));
		}

		return builder
			.AddRedis(name)
			.WithDataVolume()
			.WithHealthCheck(name);
	}
}
