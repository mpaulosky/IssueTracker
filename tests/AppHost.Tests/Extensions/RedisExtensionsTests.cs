// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     RedisExtensionsTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  AppHost
// =============================================

namespace IssueTracker.AppHost.Tests.Extensions;

/// <summary>
/// Unit tests for <see cref="RedisExtensions"/>.
/// </summary>
public class RedisExtensionsTests
{
	/// <summary>
	/// Test that AddRedisCache returns a valid resource builder.
	/// </summary>
	[Fact]
	public void AddRedisCache_WithValidBuilder_ReturnsResourceBuilder()
	{
		// Arrange
		var builder = DistributedApplication.CreateBuilder();

		// Act
		var result = builder.AddRedisCache();

		// Assert
		result.Should().NotBeNull();
	}

	/// <summary>
	/// Test that AddRedisCache with custom name returns a valid resource builder.
	/// </summary>
	[Fact]
	public void AddRedisCache_WithCustomName_ReturnsResourceBuilderWithCustomName()
	{
		// Arrange
		var builder = DistributedApplication.CreateBuilder();
		const string customName = "custom-redis";

		// Act
		var result = builder.AddRedisCache(customName);

		// Assert
		result.Should().NotBeNull();
		result.Resource.Name.Should().Be(customName);
	}

	/// <summary>
	/// Test that AddRedisCache throws when builder is null.
	/// </summary>
	[Fact]
	public void AddRedisCache_WithNullBuilder_ThrowsArgumentNullException()
	{
		// Arrange
		IDistributedApplicationBuilder? builder = null;

		// Act & Assert
		var action = () => builder!.AddRedisCache();
		action.Should().Throw<ArgumentNullException>()
			.WithParameterName("builder");
	}

	/// <summary>
	/// Test that AddRedisCache throws when name is null.
	/// </summary>
	[Fact]
	public void AddRedisCache_WithNullName_ThrowsArgumentException()
	{
		// Arrange
		var builder = DistributedApplication.CreateBuilder();

		// Act & Assert
		var action = () => builder.AddRedisCache(null!);
		action.Should().Throw<ArgumentException>()
			.WithParameterName("name");
	}

	/// <summary>
	/// Test that AddRedisCache throws when name is empty.
	/// </summary>
	[Fact]
	public void AddRedisCache_WithEmptyName_ThrowsArgumentException()
	{
		// Arrange
		var builder = DistributedApplication.CreateBuilder();

		// Act & Assert
		var action = () => builder.AddRedisCache(string.Empty);
		action.Should().Throw<ArgumentException>()
			.WithParameterName("name");
	}

	/// <summary>
	/// Test that AddRedisCache returns default "redis" name when not provided.
	/// </summary>
	[Fact]
	public void AddRedisCache_WithoutName_UsesDefaultName()
	{
		// Arrange
		var builder = DistributedApplication.CreateBuilder();

		// Act
		var result = builder.AddRedisCache();

		// Assert
		result.Resource.Name.Should().Be("redis");
	}
}
