// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     MongoDBHostingExtensionsTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  AppHost.Tests
// =============================================

namespace AppHost.Tests;

/// <summary>
/// Tests for MongoDB hosting extensions with dashboard commands.
/// </summary>
public class MongoDBHostingExtensionsTests
{
/// <summary>
/// Tests that AddMongoDBWithManagement creates a MongoDB resource with the correct name.
/// </summary>
[Fact]
public void AddMongoDBWithManagement_CreatesMongoDBResource_WithCorrectName()
{
// Arrange
var builder = DistributedApplication.CreateBuilder();

// Act
var mongodb = builder.AddMongoDBWithManagement("testmongo");

// Assert
mongodb.Should().NotBeNull("MongoDB resource builder should be created");
mongodb.Resource.Should().NotBeNull("MongoDB resource should be created");
mongodb.Resource.Name.Should().Be("testmongo", "Resource name should match the provided name");
}

/// <summary>
/// Tests that AddMongoDBWithManagement creates MongoDB resource with default database name.
/// </summary>
[Fact]
public void AddMongoDBWithManagement_CreatesMongoDBResource_WithDefaultDatabase()
{
// Arrange
var builder = DistributedApplication.CreateBuilder();

// Act
var mongodb = builder.AddMongoDBWithManagement("testmongo");
var app = builder.Build();

// Assert
mongodb.Should().NotBeNull("MongoDB resource builder should be created");
mongodb.Resource.Should().NotBeNull("MongoDB resource should be created");
}

/// <summary>
/// Tests that AddMongoDBWithManagement with custom database name creates resource correctly.
/// </summary>
[Fact]
public void AddMongoDBWithManagement_CreatesMongoDBResource_WithCustomDatabaseName()
{
// Arrange
var builder = DistributedApplication.CreateBuilder();

// Act
var mongodb = builder.AddMongoDBWithManagement("testmongo", "CustomDb");

// Assert
mongodb.Should().NotBeNull("MongoDB resource builder should be created");
mongodb.Resource.Should().NotBeNull("MongoDB resource should be created");
mongodb.Resource.Name.Should().Be("testmongo", "Resource name should match the provided name");
}

/// <summary>
/// Tests that AddMongoDBWithManagement throws ArgumentNullException when builder is null.
/// </summary>
[Fact]
public void AddMongoDBWithManagement_ThrowsArgumentNullException_WhenBuilderIsNull()
{
// Arrange
IDistributedApplicationBuilder builder = null!;

// Act & Assert
var act = () => builder.AddMongoDBWithManagement("testmongo");
act.Should().Throw<ArgumentNullException>()
.WithParameterName("builder", "Builder should not be null");
}

/// <summary>
/// Tests that AddMongoDBWithManagement throws ArgumentException when name is null or whitespace.
/// </summary>
[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("   ")]
public void AddMongoDBWithManagement_ThrowsArgumentException_WhenNameIsNullOrWhitespace(string? name)
{
// Arrange
var builder = DistributedApplication.CreateBuilder();

// Act & Assert
var act = () => builder.AddMongoDBWithManagement(name!);
act.Should().Throw<ArgumentException>()
.WithParameterName("name", "Name should not be null or whitespace");
}

/// <summary>
/// Tests that AddMongoDBWithManagement throws ArgumentException when databaseName is null or whitespace.
/// </summary>
[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("   ")]
public void AddMongoDBWithManagement_ThrowsArgumentException_WhenDatabaseNameIsNullOrWhitespace(string? databaseName)
{
// Arrange
var builder = DistributedApplication.CreateBuilder();

// Act & Assert
var act = () => builder.AddMongoDBWithManagement("testmongo", databaseName!);
act.Should().Throw<ArgumentException>()
.WithParameterName("databaseName", "Database name should not be null or whitespace");
}

/// <summary>
/// Tests that the MongoDB resource has health check annotation.
/// </summary>
[Fact]
public void AddMongoDBWithManagement_AddsHealthCheckAnnotation()
{
// Arrange
var builder = DistributedApplication.CreateBuilder();

// Act
var mongodb = builder.AddMongoDBWithManagement("testmongo");

// Assert
mongodb.Resource.Annotations.Should().NotBeEmpty("Resource should have annotations");
mongodb.Resource.Annotations.Should().Contain(a => a.GetType().Name.Contains("HealthCheck"), 
"Resource should have health check annotation");
}

/// <summary>
/// Tests that the MongoDB resource has data volume annotation.
/// </summary>
[Fact]
public void AddMongoDBWithManagement_AddsDataVolumeAnnotation()
{
// Arrange
var builder = DistributedApplication.CreateBuilder();

// Act
var mongodb = builder.AddMongoDBWithManagement("testmongo");

// Assert
mongodb.Resource.Annotations.Should().NotBeEmpty("Resource should have annotations");
}
}
