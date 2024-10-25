﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CreateStatusTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateStatusTests : IAsyncLifetime
{
	private const string CleanupValue = "statuses";

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;

	public CreateStatusTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		IMongoDbContextFactory context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new StatusRepository(context);
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{
		await _factory.ResetCollectionAsync(CleanupValue);
	}

	[Fact]
	public async Task CreateAsync_With_ValidData_Should_CreateAStatus_TestAsync()
	{
		// Arrange
		StatusModel expected = FakeStatus.GetNewStatus();

		// Act
		await _sut.CreateAsync(expected);

		// Assert
		expected.Id.Should().NotBeNull();
	}

	[Fact]
	public async Task CreateAsync_With_InValidData_Should_FailToCreateAStatus_TestAsync()
	{
		// Arrange

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateAsync(null!));
	}
}