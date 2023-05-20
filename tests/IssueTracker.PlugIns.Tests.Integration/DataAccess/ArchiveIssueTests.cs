﻿
namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class ArchiveIssueTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueRepository _sut;
	private const string CleanupValue = "issues";

	public ArchiveIssueTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new IssueRepository(context);

	}

	[Fact(DisplayName = "Archive Issue With Valid Data (Archive)")]
	public async Task ArchiveAsync_With_ValidData_Should_ArchiveAIssue_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();

		await _sut.CreateAsync(expected);

		// Act
		await _sut.ArchiveAsync(expected);

		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Archived.Should().BeTrue();

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
