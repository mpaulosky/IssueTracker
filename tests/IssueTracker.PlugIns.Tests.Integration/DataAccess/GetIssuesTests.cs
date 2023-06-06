﻿namespace IssueTracker.PlugIns.DataAccess;[ExcludeFromCodeCoverage][Collection("Test Collection")]public class GetIssuesTests : IAsyncLifetime{	private const string CleanupValue = "issues";	private readonly IssueTrackerTestFactory _factory;	private readonly IssueRepository _sut;	public GetIssuesTests(IssueTrackerTestFactory factory)	{		_factory = factory;		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();		_sut = new IssueRepository(context);	}	public Task InitializeAsync()	{		return Task.CompletedTask;	}	public async Task DisposeAsync()	{		await _factory.ResetCollectionAsync(CleanupValue);	}	[Fact]	public async Task GetAllAsync_With_ValidData_Should_ReturnIssues_Test()	{		// Arrange		var expected = FakeIssue.GetNewIssue();		await _sut.CreateAsync(expected);		// Act		var results = (await _sut.GetAllAsync()).ToList();		// Assert		results.Count.Should().Be(1);		results.First().Title.Should().Be(expected.Title);		results.First().Description.Should().Be(expected.Description);	}}