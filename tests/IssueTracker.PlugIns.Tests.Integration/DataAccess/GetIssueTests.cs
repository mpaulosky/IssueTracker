// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     GetIssueTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetIssueTests : IAsyncLifetime
{
	private const string CleanupValue = "issues";

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueRepository _sut;

	public GetIssueTests(IssueTrackerTestFactory factory)
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
	public async Task GetAsync_With_Data_Should_ReturnAValidIssue_TestAsync()
	{
		// Arrange
		IssueModel expected = FakeIssue.GetNewIssue();
		await _sut.CreateAsync(expected);

		// Act
		IssueModel result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Title.Should().Be(expected.Title);
		result.Id.Should().Be(expected.Id);
		result.Author.Id.Should().Be(expected.Author.Id);
	}

	[Theory]
	[InlineData("62cf2ad6326e99d665759e5a")]
	public async Task GetAsync_WithOutData_Should_Return_Nothing_TestAsync(string? value)
	{
		// Arrange

		// Act
		IssueModel result = await _sut.GetAsync(value!);

		// Assert
		result.Should().BeNull();
	}
}