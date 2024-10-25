﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     GetApprovedIssuesTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetApprovedIssuesTests : IAsyncLifetime
{
	private const string CleanupValue = "issues";

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueRepository _sut;

	public GetApprovedIssuesTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		IMongoDbContextFactory context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new IssueRepository(context);
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
	public async Task GetApprovedIssues_With_ValidData_Should_ReturnIssues_Test()
	{
		// Arrange
		IssueModel expected = FakeIssue.GetNewIssue();
		expected.Rejected = false;
		expected.ApprovedForRelease = true;
		expected.Archived = false;

		await _sut.CreateAsync(expected);

		List<IssueModel> results = (await _sut.GetApprovedAsync()).ToList();

		// Assert
		// Act
		results.Count.Should().Be(1);
		results.First().Title.Should().Be(expected.Title);
		results.First().Description.Should().Be(expected.Description);
	}
}