// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CacheService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  ServiceDefaults
// =============================================

namespace ServiceDefaults;

/// <summary>
/// Provides an abstraction for distributed caching operations with JSON serialization.
/// </summary>
public interface ICacheService
{
	/// <summary>
	/// Retrieves a value from the cache by key.
	/// </summary>
	/// <typeparam name="T">The type of the cached value.</typeparam>
	/// <param name="key">The cache key.</param>
	/// <returns>The cached value, or null if not found or expired.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is null or empty.</exception>
	Task<T?> GetAsync<T>(string key);

	/// <summary>
	/// Stores a value in the cache with an optional expiration time.
	/// </summary>
	/// <typeparam name="T">The type of the value to cache.</typeparam>
	/// <param name="key">The cache key.</param>
	/// <param name="value">The value to cache.</param>
	/// <param name="expiration">The absolute expiration duration. If null, no expiration is set.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is null or empty.</exception>
	Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

	/// <summary>
	/// Removes a value from the cache by key.
	/// </summary>
	/// <param name="key">The cache key.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is null or empty.</exception>
	Task RemoveAsync(string key);
}

/// <summary>
/// Implementation of ICacheService using IDistributedCache with JSON serialization.
/// </summary>
public class CacheService(
	IDistributedCache distributedCache,
	ILogger<CacheService> logger) : ICacheService
{
	/// <summary>
	/// Retrieves a value from the cache by key.
	/// </summary>
	/// <typeparam name="T">The type of the cached value.</typeparam>
	/// <param name="key">The cache key.</param>
	/// <returns>The cached value, or null if not found or expired.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is null or empty.</exception>
	public async Task<T?> GetAsync<T>(string key)
	{
		ArgumentException.ThrowIfNullOrEmpty(key);

		try
		{
			var cachedData = await distributedCache.GetStringAsync(key);

			if (cachedData is null)
			{
				logger.LogDebug("Cache miss for key: {CacheKey}", key);
				return default;
			}

			var result = System.Text.Json.JsonSerializer.Deserialize<T>(cachedData);
			logger.LogDebug("Cache hit for key: {CacheKey}", key);
			return result;
		}
		catch (System.Text.Json.JsonException ex)
		{
			logger.LogWarning(ex, "Failed to deserialize cached value for key: {CacheKey}", key);
			await distributedCache.RemoveAsync(key);
			return default;
		}
	}

	/// <summary>
	/// Stores a value in the cache with an optional expiration time.
	/// </summary>
	/// <typeparam name="T">The type of the value to cache.</typeparam>
	/// <param name="key">The cache key.</param>
	/// <param name="value">The value to cache.</param>
	/// <param name="expiration">The absolute expiration duration. If null, no expiration is set.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is null or empty.</exception>
	public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
	{
		ArgumentException.ThrowIfNullOrEmpty(key);

		try
		{
			var serialized = System.Text.Json.JsonSerializer.Serialize(value);
			var options = new DistributedCacheEntryOptions();

			if (expiration.HasValue)
			{
				options.AbsoluteExpirationRelativeToNow = expiration;
			}

			await distributedCache.SetStringAsync(key, serialized, options);
			logger.LogDebug("Cached value for key: {CacheKey} with expiration: {Expiration}", 
				key, expiration?.TotalSeconds ?? -1);
		}
		catch (System.Text.Json.JsonException ex)
		{
			logger.LogError(ex, "Failed to serialize value for cache key: {CacheKey}", key);
			throw;
		}
	}

	/// <summary>
	/// Removes a value from the cache by key.
	/// </summary>
	/// <param name="key">The cache key.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is null or empty.</exception>
	public async Task RemoveAsync(string key)
	{
		ArgumentException.ThrowIfNullOrEmpty(key);

		await distributedCache.RemoveAsync(key);
		logger.LogDebug("Removed cache entry for key: {CacheKey}", key);
	}
}
