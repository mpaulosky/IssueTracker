// Copyright (c) 2023. All rights reserved.
// File Name :     CreateIssueTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateIssueTests : IAsyncLifetime
{
	private const string CleanupValue = "issues";

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueRepository _sut;

	public CreateIssueTests(IssueTrackerTestFactory factory)
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
	public async Task CreateAsync_With_ValidData_Should_CreateAIssue_TestAsync()
	{
		// Arrange
		IssueModel expected = FakeIssue.GetNewIssue();

		// Act
		await _sut.CreateAsync(expected);

		// Assert
		expected.Id.Should().NotBeNull();
	}

	[Fact]
	public async Task CreateAsync_With_InValidData_Should_FailToCreateAIssue_TestAsync()
	{
		// Arrange

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateAsync(null!));
	}
}