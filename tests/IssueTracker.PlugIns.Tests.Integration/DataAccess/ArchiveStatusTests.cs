﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ArchiveStatusTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class ArchiveStatusTests : IAsyncLifetime
{
	private const string CleanupValue = "statuses";

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;

	public ArchiveStatusTests(IssueTrackerTestFactory factory)
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

	[Fact(DisplayName = "Archive Status With Valid Data (Archive)")]
	public async Task ArchiveAsync_With_ValidData_Should_ArchiveAStatus_TestAsync()
	{
		// Arrange
		StatusModel expected = FakeStatus.GetNewStatus();

		await _sut.CreateAsync(expected);

		// Act
		await _sut.ArchiveAsync(expected);

		StatusModel result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Archived.Should().BeTrue();
	}
}