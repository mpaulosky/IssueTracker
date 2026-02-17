// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CacheAdminHelperTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  AppHost
// =============================================

namespace IssueTracker.AppHost.Tests.Helpers;

/// <summary>
/// Unit tests for <see cref="CacheAdminHelper"/>.
/// </summary>
public class CacheAdminHelperTests
{
	/// <summary>
	/// Test that ClearRedisDatabaseAsync throws when connection string is null.
	/// </summary>
	[Fact]
	public async Task ClearRedisDatabaseAsync_WithNullConnectionString_ThrowsArgumentException()
	{
		// Act & Assert
		await FluentActions.Invoking(() => CacheAdminHelper.ClearRedisDatabaseAsync(null!))
			.Should().ThrowAsync<ArgumentException>()
			.WithParameterName("connectionString");
	}

	/// <summary>
	/// Test that ClearRedisDatabaseAsync throws when connection string is empty.
	/// </summary>
	[Fact]
	public async Task ClearRedisDatabaseAsync_WithEmptyConnectionString_ThrowsArgumentException()
	{
		// Act & Assert
		await FluentActions.Invoking(() => CacheAdminHelper.ClearRedisDatabaseAsync(string.Empty))
			.Should().ThrowAsync<ArgumentException>()
			.WithParameterName("connectionString");
	}

	/// <summary>
	/// Test that ClearRedisDatabaseAsync throws when database number is negative.
	/// </summary>
	[Fact]
	public async Task ClearRedisDatabaseAsync_WithNegativeDatabase_ThrowsArgumentException()
	{
		// Act & Assert
		await FluentActions.Invoking(() => CacheAdminHelper.ClearRedisDatabaseAsync("localhost:6379", -1))
			.Should().ThrowAsync<ArgumentException>()
			.WithParameterName("database");
	}

	/// <summary>
	/// Test that ClearAllRedisDatabasesAsync throws when connection string is null.
	/// </summary>
	[Fact]
	public async Task ClearAllRedisDatabasesAsync_WithNullConnectionString_ThrowsArgumentException()
	{
		// Act & Assert
		await FluentActions.Invoking(() => CacheAdminHelper.ClearAllRedisDatabasesAsync(null!))
			.Should().ThrowAsync<ArgumentException>()
			.WithParameterName("connectionString");
	}

	/// <summary>
	/// Test that ClearAllRedisDatabasesAsync throws when connection string is empty.
	/// </summary>
	[Fact]
	public async Task ClearAllRedisDatabasesAsync_WithEmptyConnectionString_ThrowsArgumentException()
	{
		// Act & Assert
		await FluentActions.Invoking(() => CacheAdminHelper.ClearAllRedisDatabasesAsync(string.Empty))
			.Should().ThrowAsync<ArgumentException>()
			.WithParameterName("connectionString");
	}

	/// <summary>
	/// Test that GetRedisInfoAsync throws when connection string is null.
	/// </summary>
	[Fact]
	public async Task GetRedisInfoAsync_WithNullConnectionString_ThrowsArgumentException()
	{
		// Act & Assert
		await FluentActions.Invoking(() => CacheAdminHelper.GetRedisInfoAsync(null!))
			.Should().ThrowAsync<ArgumentException>()
			.WithParameterName("connectionString");
	}

	/// <summary>
	/// Test that GetRedisInfoAsync throws when connection string is empty.
	/// </summary>
	[Fact]
	public async Task GetRedisInfoAsync_WithEmptyConnectionString_ThrowsArgumentException()
	{
		// Act & Assert
		await FluentActions.Invoking(() => CacheAdminHelper.GetRedisInfoAsync(string.Empty))
			.Should().ThrowAsync<ArgumentException>()
			.WithParameterName("connectionString");
	}
}
