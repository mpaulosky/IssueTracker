﻿namespace IssueTracker.Library.Services.IssueServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateIssueTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;
	private string _cleanupValue;

	public UpdateIssueTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task UpdateIssue_With_ValidData_Should_UpdateTheIssue_Test()
	{

		// Arrange
		_cleanupValue = "issues";
		var expected = FakeIssue.GetNewIssue();
		await _sut.CreateIssue(expected);

		// Act
		expected.Description = "Updated";
		await _sut.UpdateIssue(expected);
		var result = await _sut.GetIssue(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Description.Should().Be(expected.Description);

	}

	[Fact]
	public async Task UpdateIssue_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";
		IssueModel expected = null;

		// Act
		var act = async () => await _sut.UpdateIssue(expected);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	public Task InitializeAsync() => Task.CompletedTask;

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}
