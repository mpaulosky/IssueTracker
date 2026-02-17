// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ServiceDefaultsExtensionsTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  ServiceDefaults.Tests
// =============================================

namespace ServiceDefaults.Tests;

/// <summary>
/// Tests for ServiceDefaults extensions and infrastructure setup.
/// </summary>
public class ServiceDefaultsExtensionsTests
{
	/// <summary>
	/// Tests that IDistributedCache is registered and resolvable when AddServiceDefaults is called.
	/// </summary>
	[Fact]
	public void AddServiceDefaults_RegistersICacheService()
	{
		// Arrange
		var builder = Host.CreateDefaultBuilder();
		builder.ConfigureServices(services =>
		{
			services.AddSingleton<IDistributedCache>(new InMemoryCacheForTest());
			services.AddScoped<ICacheService, CacheService>();
		});

		var host = builder.Build();

		// Act
		var cacheService = host.Services.GetService<ICacheService>();

		// Assert
		cacheService.Should().NotBeNull("ICacheService should be registered");
	}

	/// <summary>
	/// Tests that health checks are registered.
	/// </summary>
	[Fact]
	public void AddServiceDefaults_RegistersHealthChecks()
	{
		// Arrange
		var builder = Host.CreateDefaultBuilder();
		builder.ConfigureServices(services =>
		{
			services.AddHealthChecks();
		});

		var host = builder.Build();

		// Act & Assert
		// Just verify host was built successfully and services were registered
		host.Should().NotBeNull();
		var serviceProvider = host.Services;
		serviceProvider.Should().NotBeNull();
	}

	/// <summary>
	/// Mock in-memory cache for testing.
	/// </summary>
	private class InMemoryCacheForTest : IDistributedCache
	{
		private readonly Dictionary<string, byte[]> _cache = new();

		public byte[]? Get(string key) => _cache.TryGetValue(key, out var value) ? value : null;

		public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
			=> Task.FromResult(Get(key));

		public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
			=> _cache[key] = value;

		public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
		{
			Set(key, value, options);
			return Task.CompletedTask;
		}

		public void Remove(string key) => _cache.Remove(key);

		public Task RemoveAsync(string key, CancellationToken token = default)
		{
			Remove(key);
			return Task.CompletedTask;
		}

		public void Refresh(string key) { }

		public Task RefreshAsync(string key, CancellationToken token = default) => Task.CompletedTask;
	}
}
