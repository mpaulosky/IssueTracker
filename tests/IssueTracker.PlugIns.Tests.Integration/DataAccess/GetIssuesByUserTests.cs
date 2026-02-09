// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     GetIssuesByUserTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetIssuesByUserTests : IAsyncLifetime
{
	private const string CleanupValue = "issues";

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueRepository _sut;

	public GetIssuesByUserTests(IssueTrackerTestFactory factory)
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
		await _factory.ResetDatabaseAsync();
	}

	[Fact]
	public async Task GetByUserAsync_With_ValidData_Should_ReturnIssues_Test()
	{
		// Arrange
		IssueModel expected = FakeIssue.GetNewIssue();
		await _sut.CreateAsync(expected);

		// Act
		List<IssueModel> results = (await _sut.GetByUserAsync(expected.Author.Id)).ToList();

		// Assert
		results.Count.Should().Be(1);
		results.First().Title.Should().Be(expected.Title);
		results.First().Description.Should().Be(expected.Description);
		results.First().Author.Id.Should().Be(expected.Author.Id);
	}
}