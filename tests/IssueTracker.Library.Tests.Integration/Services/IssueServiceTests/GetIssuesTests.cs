﻿namespace IssueTracker.Library.Services.IssueServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetIssuesTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;

	public GetIssuesTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;

		var db = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
		db.Database.DropCollection(CollectionNames.GetCollectionName(nameof(IssueModel)));

		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		memCache.Remove("IssueData");

		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task GetIssues_With_ValidData_Should_ReturnIssues_Test()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();
		await _sut.CreateIssue(expected);

		// Act
		var results = await _sut.GetIssues();

		// Assert
		results.Count.Should().Be(1);
		results.First().IssueName.Should().Be(expected.IssueName);
		results.First().Description.Should().Be(expected.Description);

	}

}