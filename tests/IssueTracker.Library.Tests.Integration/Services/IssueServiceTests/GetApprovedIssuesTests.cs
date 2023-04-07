﻿using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
using IssueTracker.PlugIns.Services;

namespace IssueTracker.PlugIns.Mongo.Services.IssueServiceTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetApprovedIssuesTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;
	private string _cleanupValue;


	public GetApprovedIssuesTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task GetApprovedIssues_With_ValidData_Should_ReturnIssues_Test()
	{

		// Arrange
		_cleanupValue = "issues";

		IssueModel expected = FakeIssue.GetNewIssue();
		expected.Rejected = false;
		expected.ApprovedForRelease = true;
		expected.Archived = false;

		await _sut.CreateIssue(expected);

		List<IssueModel> results = await _sut.GetApprovedIssues();

		// Assert
		// Act
		results.Count.Should().Be(1);
		results.First().Title.Should().Be(expected.Title);
		results.First().Description.Should().Be(expected.Description);

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(_cleanupValue);

	}

}