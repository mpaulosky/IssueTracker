﻿namespace IssueTracker.Library.Services.IssueServiceTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetIssuesWaitingForApprovalTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;

	public GetIssuesWaitingForApprovalTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task GetIssuesWaitingForApproval_With_ValidData_Should_ReturnIssues_Test()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();
		expected.Rejected = false;
		expected.ApprovedForRelease = false;

		await _sut.CreateIssue(expected);

		// Act
		var results = await _sut.GetIssuesWaitingForApproval();

		// Assert
		results.Count.Should().Be(1);
		results.First().IssueName.Should().Be(expected.IssueName);
		results.First().Description.Should().Be(expected.Description);

	}

}