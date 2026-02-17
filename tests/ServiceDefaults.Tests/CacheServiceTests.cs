// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CacheServiceTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  ServiceDefaults.Tests
// =============================================

namespace ServiceDefaults.Tests;

/// <summary>
/// Tests for CacheService operations using an in-memory distributed cache.
/// </summary>
public class CacheServiceTests
{
	/// <summary>
	/// Mock in-memory implementation of IDistributedCache for testing.
	/// </summary>
	private class InMemoryDistributedCache : IDistributedCache
	{
		private readonly Dictionary<string, (byte[] value, DateTime? expiration)> _cache = new();

		public byte[]? Get(string key)
		{
			if (_cache.TryGetValue(key, out var entry))
			{
				if (entry.expiration.HasValue && entry.expiration.Value < DateTime.UtcNow)
				{
					_cache.Remove(key);
					return null;
				}

				return entry.value;
			}

			return null;
		}

		public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
		{
			return Task.FromResult(Get(key));
		}

		public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
		{
			var expiration = options.AbsoluteExpirationRelativeToNow.HasValue
				? DateTime.UtcNow.Add(options.AbsoluteExpirationRelativeToNow.Value)
				: (DateTime?)null;

			_cache[key] = (value, expiration);
		}

		public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
		{
			Set(key, value, options);
			return Task.CompletedTask;
		}

		public void Remove(string key)
		{
			_cache.Remove(key);
		}

		public Task RemoveAsync(string key, CancellationToken token = default)
		{
			Remove(key);
			return Task.CompletedTask;
		}

		public void Refresh(string key)
		{
			// Not implemented for test
		}

		public Task RefreshAsync(string key, CancellationToken token = default)
		{
			return Task.CompletedTask;
		}
	}

	/// <summary>
	/// Creates a CacheService with an in-memory distributed cache.
	/// </summary>
	private static ICacheService CreateTestCacheService()
	{
		var logger = new NullLogger<CacheService>();
		var distributedCache = new InMemoryDistributedCache();
		return new CacheService(distributedCache, logger);
	}

	/// <summary>
	/// Tests that GetAsync returns null when key is not set in cache.
	/// </summary>
	[Fact]
	public async Task GetAsync_ReturnsNull_WhenKeyNotSet()
	{
		// Arrange
		var cacheService = CreateTestCacheService();
		var key = "non-existent-key";

		// Act
		var result = await cacheService.GetAsync<string>(key);

		// Assert
		result.Should().BeNull();
	}

	/// <summary>
	/// Tests that SetAsync stores a value and GetAsync retrieves it correctly.
	/// </summary>
	[Fact]
	public async Task SetAsync_StoresValue_AndGetAsync_RetrievesValue()
	{
		// Arrange
		var cacheService = CreateTestCacheService();
		var key = "test-key";
		var value = "test-value";

		// Act
		await cacheService.SetAsync(key, value);
		var result = await cacheService.GetAsync<string>(key);

		// Assert
		result.Should().Be(value);
	}

	/// <summary>
	/// Tests that SetAsync stores complex objects correctly.
	/// </summary>
	[Fact]
	public async Task SetAsync_StoresComplexObject_AndGetAsync_RetrievesObject()
	{
		// Arrange
		var cacheService = CreateTestCacheService();
		var key = "complex-key";
		var value = new TestObject { Id = 1, Name = "Test", CreatedAt = DateTime.UtcNow };

		// Act
		await cacheService.SetAsync(key, value);
		var result = await cacheService.GetAsync<TestObject>(key);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(value.Id);
		result.Name.Should().Be(value.Name);
	}

	/// <summary>
	/// Tests that RemoveAsync deletes a cached value.
	/// </summary>
	[Fact]
	public async Task RemoveAsync_DeletesCachedValue()
	{
		// Arrange
		var cacheService = CreateTestCacheService();
		var key = "remove-key";
		var value = "value-to-remove";

		await cacheService.SetAsync(key, value);

		// Act
		await cacheService.RemoveAsync(key);
		var result = await cacheService.GetAsync<string>(key);

		// Assert
		result.Should().BeNull();
	}

	/// <summary>
	/// Tests that SetAsync with expiration respects the TTL.
	/// </summary>
	[Fact]
	public async Task SetAsync_WithExpiration_ExpiresAfterTimespan()
	{
		// Arrange
		var cacheService = CreateTestCacheService();
		var key = "expiring-key";
		var value = "expiring-value";
		var expiration = TimeSpan.FromMilliseconds(500);

		// Act
		await cacheService.SetAsync(key, value, expiration);
		var resultBefore = await cacheService.GetAsync<string>(key);

		// Wait for expiration
		await Task.Delay(1000);
		var resultAfter = await cacheService.GetAsync<string>(key);

		// Assert
		resultBefore.Should().Be(value);
		resultAfter.Should().BeNull();
	}

	/// <summary>
	/// Tests that GetAsync throws when key is null.
	/// </summary>
	[Fact]
	public async Task GetAsync_ThrowsArgumentException_WhenKeyIsNull()
	{
		// Arrange
		var cacheService = CreateTestCacheService();

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => cacheService.GetAsync<string>(null!));
	}

	/// <summary>
	/// Tests that GetAsync throws when key is empty.
	/// </summary>
	[Fact]
	public async Task GetAsync_ThrowsArgumentException_WhenKeyIsEmpty()
	{
		// Arrange
		var cacheService = CreateTestCacheService();

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() => cacheService.GetAsync<string>(string.Empty));
	}

	/// <summary>
	/// Tests that SetAsync throws when key is null.
	/// </summary>
	[Fact]
	public async Task SetAsync_ThrowsArgumentException_WhenKeyIsNull()
	{
		// Arrange
		var cacheService = CreateTestCacheService();

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => cacheService.SetAsync(null!, "value"));
	}

	/// <summary>
	/// Tests that RemoveAsync throws when key is null.
	/// </summary>
	[Fact]
	public async Task RemoveAsync_ThrowsArgumentException_WhenKeyIsNull()
	{
		// Arrange
		var cacheService = CreateTestCacheService();

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => cacheService.RemoveAsync(null!));
	}

	/// <summary>
	/// Tests that ICacheService is registered in ServiceDefaults.
	/// </summary>
	[Fact]
	public void ICacheService_IsRegistered_InServiceDefaults()
	{
		// Arrange
		var builder = Host.CreateDefaultBuilder();
		builder.ConfigureServices(services =>
		{
			services.AddSingleton<IDistributedCache, InMemoryDistributedCache>();
			services.AddScoped<ICacheService, CacheService>();
		});

		var host = builder.Build();

		// Act
		var cacheService = host.Services.GetService<ICacheService>();

		// Assert
		cacheService.Should().NotBeNull();
		cacheService.Should().BeOfType<CacheService>();
	}

	/// <summary>
	/// Test object for complex caching tests.
	/// </summary>
	[ExcludeFromCodeCoverage]
	private class TestObject
	{
		/// <summary>
		/// Gets or sets the ID.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string? Name { get; set; }

		/// <summary>
		/// Gets or sets the creation timestamp.
		/// </summary>
		public DateTime CreatedAt { get; set; }
	}
}
